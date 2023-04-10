using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoSingleton<PlayerController>
{
    /*Karakterin hareketini ve animasyonlarını bu script ile kontrol ediyoruz. Sahnede sadece bir tane olması gerekiyor. Singleton yapıda olduğu için.
    Asıl saldırı kodları Weapon sınıfında açıklamasına oradan bakabilirsiniz*/
    Rigidbody2D rb;
    Player player;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = Player.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("AnimSpeed", player.moveSpeed / 10);
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.weaponType == player.currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        Move(InputManager.Instance.movement);
        Aim(InputManager.Instance.aim);
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * player.moveSpeed;
        player.lastDirection = GetDirection(direction);

        if (animator == null)
            return;

        if (direction == Vector2.zero)
        {
            animator.SetBool("isRunning", false);
            return;
        }

        animator.SetBool("isRunning", true);

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
            return;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            return;
        }
        else
        {
            if (direction.y > 0)
            {
                spriteRenderer.flipX = false;
                return;
            }
            else if (direction.y < 0)
            {
                spriteRenderer.flipX = true;
                return;
            }
            else
            {
                return;
            }
        }
    }

    public void Aim(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        player.lastAttackDirection = GetDirection(direction);
    }

    public Direction GetDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                return Direction.Right;
            }
            else if (direction.x < 0)
            {
                return Direction.Left;
            }
        }
        else
        {
            if (direction.y > 0)
            {
                return Direction.Up;
            }
            else if (direction.y < 0)
            {
                return Direction.Down;
            }
        }
        return Direction.Right;
    }

    public void Dash(Vector2 direction)
    {
        if (player.dashCooldown)
            return;

        player.dashCooldown = true;
        player.dashAmount = 0;
        StartCoroutine(DashCooldown());

        transform.DOMove(transform.position + (Vector3)direction * player.dashDistance, 0.2f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            DashAttack();
            player.dashEffect.SetActive(true);
            player.dashEffect.transform.position = transform.position;
            animator.SetTrigger("Dash");
            player.dashEffect.transform.DOScale(0.5f, 0.2f).SetEase(Ease.InElastic).OnComplete(() =>
            {
                player.dashEffect.SetActive(false);
                player.dashEffect.transform.localScale = Vector3.one;
            }).SetDelay(0.5f);
        });
    }

    IEnumerator DashCooldown()
    {
        float Timer = 0;
        while (Timer < player.dashCooldownTime)
        {
            Timer += Time.deltaTime;
            player.dashAmount = (int)(Timer / player.dashCooldownTime);
            yield return null;
        }
        player.dashCooldown = false;
        player.dashAmount = 100;
    }

    public void SpecialAttack()
    {
        if (player.xpAmount < player.maxXP)
            return;

        player.xpAmount = 0;

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, player.specialRange);

        foreach (Collider2D c in hit)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RemoveHealth(player.specialDamage);
                Vector2 dir = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<EnemyController>().Knockback(dir, player.specialKnockback);
            }
        }
    }

    public void DashAttack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, player.dashDistance);

        foreach (Collider2D c in hit)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.RemoveHealth(player.dashDamage);
                Vector2 dir = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<EnemyController>().Knockback(dir, player.dashDistance * 4);
            }
        }
    }
}
