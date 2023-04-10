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
    private Transform goToAfterSpawn;
    private int minHealthReward = 0;
    private int maxHealthReward = 20;
    private int chanceToDropHealth = 10;
    private int minXPReward = 0;
    private int maxXPReward = 20;
    private int chanceToDropXP = 20;
    private int minArmorReward = 0;
    private int maxArmorReward = 20;
    private int chanceToDropArmor = 5;
    private bool isKnockedBack = false;

    public void Awake()
    {
        player = Player.Instance;
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (isKnockedBack)
            return;

        if (justCreated)
        {
            distanceToPlayer = Vector2.Distance(transform.position, goToAfterSpawn.position);
            if (distanceToPlayer <= 1)
            {
                justCreated = false;
                return;
            }
            Move((goToAfterSpawn.position - transform.position).normalized);
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
        else
        {
            Move((goToAfterSpawn.position - transform.position).normalized);
        }
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * enemy.movementSpeed;
    }

    IEnumerator AttackPlayer()
    {
        while (isAttacking)
        {
            if (player.currentArmor > 0)
            {
                player.RemoveArmor(enemy.damage);
            }
            else
            {
                player.RemoveHealth(enemy.damage);
            }
            yield return new WaitForSeconds(enemy.attackSpeed);
        }
    }

    public void RestoreHealth(Transform goToAfterSpawn)
    {
        enemy.health = 100;
        this.goToAfterSpawn = goToAfterSpawn;
        justCreated = true;
    }

    public void Knockback(Vector2 direction, float force)
    {
        if (isKnockedBack)
            return;
        isKnockedBack = true;
        if (gameObject.activeSelf)
            StartCoroutine(KnockbackCooldown());
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    IEnumerator KnockbackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isKnockedBack = false;
    }

    public void DeathReward()
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random <= chanceToDropArmor)
        {
            int armorReward = UnityEngine.Random.Range(minArmorReward, maxArmorReward);
            player.AddArmor(ref armorReward); // passive item için ref eklendi
            GameUIController.Instance.PushMessage("Armor +" + armorReward);
        }
        else if (random <= chanceToDropArmor + chanceToDropHealth)
        {
            int healthReward = UnityEngine.Random.Range(minHealthReward, maxHealthReward);
            player.AddHealth(healthReward);
            GameUIController.Instance.PushMessage("Health +" + healthReward);
        }
        else if (random <= chanceToDropArmor + chanceToDropHealth + chanceToDropXP)
        {
            int XPReward = UnityEngine.Random.Range(minXPReward, maxXPReward);
            player.AddXP(XPReward);
            GameUIController.Instance.PushMessage("XP +" + XPReward);
        }
    }
}
