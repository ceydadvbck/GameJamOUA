using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /*Düşmanların özelliklerini tutan script. Player scripti ile benzer şekilde. Tek fark Attack kısmı için EnemyController scripti kullanılıyor.
    Player gibi farklı Weaponlara sahip olmayacağı için bunda direkt Controller üzerinden Attack fonksiyonunu çalıştırmak daha mantıklı geldi.*/
    public EnemyType enemyType; //Düşman tipini enum ile kontrol ediyoruz. Attack kısmında çok işimize yarayacak.
    public float health;
    public float damage;
    public float attackSpeed;
    public float range;
    public float movementSpeed;
    private float lastAttackTime;
    private Player player;

    public void Start()
    {
        player = Player.Instance;
    }

    public void DecreaseHealth(float damage)
    {
        health -= damage;
        if (health <= 0)
        {

        }
    }
}
