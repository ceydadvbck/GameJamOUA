using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Düşmanların hareketlerini ve animasyonlarını kontrol eden script. Düşmanın Attack fonksiyonunu burada çağırıyoruz.
    Rigidbody2D rb;
    public Enemy enemy;
    private Player player;
    private float distanceToPlayer;
    private bool isAttacking;

    public void Start()
    {
        player = Player.Instance;
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < enemy.playerDetectionDistance && !isAttacking)
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemy.movementSpeed * Time.deltaTime);
            rb.velocity = (player.transform.position - transform.position).normalized * enemy.movementSpeed * Time.deltaTime;
            if (distanceToPlayer <= enemy.attackDistance)
            {
                isAttacking = true;
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            isAttacking = false;
            rb.velocity = Vector2.zero;
        }
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * enemy.movementSpeed * Time.deltaTime;
    }

    IEnumerator AttackPlayer()
    {
        while (isAttacking)
        {
            player.RemoveHealth(enemy.damage);
            yield return new WaitForSeconds(enemy.attackSpeed);
        }
    }
}
