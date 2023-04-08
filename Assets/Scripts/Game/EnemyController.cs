using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Düşmanların hareketlerini ve animasyonlarını kontrol eden script. Düşmanın Attack fonksiyonunu burada çağırıyoruz.
    Rigidbody2D rb;
    public Enemy enemy;
    private Player player;

    public void Start()
    {
        player = Player.Instance;
    }

    public void Update()
    {

    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * enemy.movementSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        //EnemyType enum'u üzerinden kontrol edip ona göre bir attack gerçekleştireceğiz.
    }
}
