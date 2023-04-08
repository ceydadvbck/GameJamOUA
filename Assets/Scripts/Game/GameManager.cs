using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    /*Oyunun genel yönetiminin yapıldığı script bu. Düşmanlar için object pooling kullanıyoruz. Maksimum sayıyı belirleyebiliriz.
    Tüm spawn işlemleri bu scriptten çağrılarak yapılacak. Gerekirse fonksiyonları geri dönüş değeri olacak şekilde değiştirebiliriz.*/
    public GameObject[] enemyPrefabs;
    public GameObject[] weaponPrefabs;
    public GameObject[] weaponUpgradePrefabs;
    public GameObject[] playerUpgradePrefabs;
    public Transform spawnPoint;
    public Transform goToAfterSpawn;
    public Vector2 spawnRange;
    public float spawnRate;
    private float lastSpawnTime;
    public int maxEnemyCountByType;
    public Stack<GameObject> meleeEnemiesStack = new Stack<GameObject>();
    public Stack<GameObject> rangedEnemiesStack = new Stack<GameObject>();

    public void Start()
    {
        for (int i = 0; i < maxEnemyCountByType; i++)
        {
            Transform melee = Instantiate(enemyPrefabs[0], transform).GetComponent<Transform>();
            meleeEnemiesStack.Push(melee.gameObject);
            melee.gameObject.SetActive(false);
            Transform ranged = Instantiate(enemyPrefabs[1], transform).GetComponent<Transform>();
            rangedEnemiesStack.Push(ranged.gameObject);
            ranged.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (Time.time - lastSpawnTime > spawnRate)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    public void SpawnEnemy()
    {
        EnemyType enemyType = (EnemyType)Random.Range(0, 2);
        if (enemyType == EnemyType.Melee)
        {
            if (meleeEnemiesStack.Count > 0)
            {
                GameObject enemy = meleeEnemiesStack.Pop();
                enemy.SetActive(true);
                enemy.transform.position =  new Vector2(spawnPoint.position.x + Random.Range(-spawnRange.x, spawnRange.x), spawnPoint.position.y + Random.Range(-spawnRange.y, spawnRange.y));
                enemy.GetComponent<EnemyController>().RestoreHealth();
            }
        }
        else if (enemyType == EnemyType.Ranged)
        {
            if (rangedEnemiesStack.Count > 0)
            {
                GameObject enemy = rangedEnemiesStack.Pop();
                enemy.gameObject.SetActive(true);
                enemy.transform.position = new Vector2(spawnPoint.position.x + Random.Range(-spawnRange.x, spawnRange.x), spawnPoint.position.y + Random.Range(-spawnRange.y, spawnRange.y));
                enemy.GetComponent<EnemyController>().RestoreHealth();
            }
        }
    }

    public void SpawnWeapon()
    {

    }

    public void SpawnWeaponUpgrade()
    {

    }

    public void SpawnPlayerUpgrade()
    {

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
    }
}
