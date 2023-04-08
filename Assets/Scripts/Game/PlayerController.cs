using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //animator = GetComponent<Animator>();
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * player.moveSpeed * Time.deltaTime;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                player.lastDirection = AttackDirection.Right;
            }
            else if(direction.x < 0)
            {
                player.lastDirection = AttackDirection.Left;
            }
        }
        else
        {
            if (direction.y > 0)
            {
                player.lastDirection = AttackDirection.Up;
            }
            else if(direction.y < 0)
            {
                player.lastDirection = AttackDirection.Down;
            }
        }
        /*animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);*/
    }
}
