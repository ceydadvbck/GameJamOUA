using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    //Düşmanların hareketlerini ve animasyonlarını kontrol eden script. Düşmanın Attack fonksiyonunu burada çağırıyoruz.
    Rigidbody2D rb;
    [NonSerialized] public Enemy enemy;
    private Player player;
    private float distanceToPlayer;
    private bool isAttacking;
    private Coroutine attackCoroutine = null;
    private bool justCreated = true;

    public void Awake()
    {
        player = Player.Instance;
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (justCreated)
        {
            distanceToPlayer = Vector2.Distance(transform.position, GameManager.Instance.goToAfterSpawn.position);
            if (distanceToPlayer <= 1)
            {
                justCreated = false;
                return;
            }
            Move((GameManager.Instance.goToAfterSpawn.position - transform.position).normalized);
            return;
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < enemy.playerDetectionDistance)
        {
            Move((player.transform.position - transform.position).normalized);
            if (distanceToPlayer <= enemy.attackDistance && !isAttacking)
            {
                isAttacking = true;
                attackCoroutine = StartCoroutine(AttackPlayer());
            }
            else if (distanceToPlayer > enemy.attackDistance && isAttacking)
            {
                isAttacking = false;
                rb.velocity = Vector2.zero;
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
            }
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

    public void RestoreHealth()
    {
        enemy.health = 100;
        justCreated = true;
    }
}
