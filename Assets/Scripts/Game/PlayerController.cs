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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = Player.Instance;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move(InputManager.Instance.movement);
        Aim(InputManager.Instance.aim);
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * player.moveSpeed * Time.deltaTime;
        player.lastDirection = GetDirection(direction);

        if (animator == null)
            return;

        if (direction == Vector2.zero)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            return;
        }

        animator.SetFloat("Horizontal", player.lastDirection == Direction.Right ? 1 : player.lastDirection == Direction.Left ? -1 : 0);
        animator.SetFloat("Vertical", player.lastDirection == Direction.Up ? 1 : player.lastDirection == Direction.Down ? -1 : 0);
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
        if(player.dashCooldown)
            return;

        player.dashCooldown = true;
        player.dashAmount = 0;
        StartCoroutine(DashCooldown());

        transform.DOMove(transform.position + (Vector3)direction * player.dashDistance, 0.2f).SetEase(Ease.InElastic);
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
}
