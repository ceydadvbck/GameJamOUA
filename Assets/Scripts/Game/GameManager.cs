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
    public int maxEnemyCountByType;
    public Stack<EnemyController> meleeEnemiesStack = new Stack<EnemyController>();
    public Stack<EnemyController> rangedEnemiesStack = new Stack<EnemyController>();

    public void Start()
    {
        for (int i = 0; i < maxEnemyCountByType; i++)
        {
            Transform melee = Instantiate(enemyPrefabs[0], transform).GetComponent<Transform>();
            meleeEnemiesStack.Push(melee.GetComponent<EnemyController>());
            melee.gameObject.SetActive(false);
            Transform ranged = Instantiate(enemyPrefabs[1], transform).GetComponent<Transform>();
            rangedEnemiesStack.Push(ranged.GetComponent<EnemyController>());
            ranged.gameObject.SetActive(false);
        }
    }

    public void SpawnEnemy()
    {

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
}
