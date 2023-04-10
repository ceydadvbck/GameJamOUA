using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    /*Oyunun genel yönetiminin yapıldığı script bu. Düşmanlar için object pooling kullanıyoruz. Maksimum sayıyı belirleyebiliriz.
    Tüm spawn işlemleri bu scriptten çağrılarak yapılacak. Gerekirse fonksiyonları geri dönüş değeri olacak şekilde değiştirebiliriz.*/
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoint;
    public Transform[] goToAfterSpawn;
    public Vector2 spawnRange;
    public float spawnRate;
    private float lastSpawnTime;
    public int maxEnemyCountByType;
    public Stack<GameObject> meleeEnemiesStack = new Stack<GameObject>();
    public Stack<GameObject> rangedEnemiesStack = new Stack<GameObject>();
    public int enemyCountToWin = 2;
    [System.NonSerialized] public bool isPaused = false;
    public int killedEnemyCountFromStart = 0;
    public Upgrade[] activeLevelRewards;
    [System.NonSerialized] public bool isWin = false;

    void Awake()
    {
        DG.Tweening.DOTween.SetTweensCapacity(500, 50);
    }
    public void Start()
    {
        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = Player.Instance.transform;
        for (int i = 0; i < maxEnemyCountByType; i++)
        {
            Transform melee = Instantiate(enemyPrefabs[0], transform).GetComponent<Transform>();
            meleeEnemiesStack.Push(melee.gameObject);
            melee.gameObject.SetActive(false);
            Transform ranged = Instantiate(enemyPrefabs[1], transform).GetComponent<Transform>();
            rangedEnemiesStack.Push(ranged.gameObject);
            ranged.gameObject.SetActive(false);
        }
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = 0;
            StartCoroutine(AudioFadeIn(audioSource, 1));
        }
    }

    public void Update()
    {
        if (isWin)
        {
            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                child.gameObject.SetActive(false);
            }
            return;
        }
        if (Time.time - lastSpawnTime > spawnRate)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
        if (killedEnemyCountFromStart >= enemyCountToWin)
        {
            isPaused = true;
            Time.timeScale = 0;
            GameUIController.Instance.Win();
            isWin = true;
        }
    }

    IEnumerator AudioFadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    public void SpawnEnemy()
    {
        EnemyType enemyType = (EnemyType)Random.Range(0, 2);
        int index = Random.Range(0, spawnPoint.Length);
        if (spawnPoint.Length == 1)
            index = 0;
        if (enemyType == EnemyType.Melee)
        {
            if (meleeEnemiesStack.Count > 0)
            {
                GameObject enemy = meleeEnemiesStack.Pop();
                enemy.SetActive(true);
                enemy.transform.position = new Vector2(spawnPoint[index].position.x + Random.Range(-spawnRange.x, spawnRange.x), spawnPoint[index].position.y + Random.Range(-spawnRange.y, spawnRange.y));
                enemy.GetComponent<EnemyController>().RestoreHealth(goToAfterSpawn[index]);
            }
        }
        else if (enemyType == EnemyType.Ranged)
        {
            if (rangedEnemiesStack.Count > 0)
            {
                GameObject enemy = rangedEnemiesStack.Pop();
                enemy.gameObject.SetActive(true);
                enemy.transform.position = new Vector2(spawnPoint[index].position.x + Random.Range(-spawnRange.x, spawnRange.x), spawnPoint[index].position.y + Random.Range(-spawnRange.y, spawnRange.y));
                enemy.GetComponent<EnemyController>().RestoreHealth(goToAfterSpawn[index]);
            }
        }
    }

    public void Push(GameObject enemy)
    {
        EnemyType enemyType = enemy.GetComponent<Enemy>().enemyType;
        if (enemyType == EnemyType.Melee)
        {
            meleeEnemiesStack.Push(enemy);
        }
        else if (enemyType == EnemyType.Ranged)
        {
            rangedEnemiesStack.Push(enemy);
        }
        enemy.gameObject.SetActive(false);
        killedEnemyCountFromStart++;
    }

    public void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public int GetScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    public void Pause(bool usePauseMenu = true)
    {
        isPaused = true;
        Time.timeScale = 0;
        if (usePauseMenu)
            GameUIController.Instance.Pause();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        isPaused = true;
        Time.timeScale = 0;
        GameUIController.Instance.GameOver();
    }
}
