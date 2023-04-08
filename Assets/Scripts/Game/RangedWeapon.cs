using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 0f;
    private int damage = 0;
    private float range = 0f;
    private Weapon parentWeapon;
    private bool isActivated;
    private bool isHit;

    public void SetNewAttack(Weapon parentWeapon, Vector2 dir, float speed, int damage, float range)
    {
        this.parentWeapon = parentWeapon;
        direction = dir;
        this.speed = speed;
        this.damage = damage;
        this.range = range;
        transform.localScale = Vector3.one;
        isActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            transform.position += direction * speed * Time.deltaTime;
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 0.3f);// Bu kısmı saldırının bölgesine göre değiştire biliriz


            foreach (Collider2D c in hit)
            {
                Enemy enemy = c.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.RemoveHealth(damage);
                    isHit = true;
                    break;

                }
            }
            if (isHit || Vector3.Distance(transform.position, parentWeapon.transform.position) > range)
            {
                transform.localScale = Vector3.zero;
                transform.localPosition = Vector3.zero;
                direction = Vector3.zero;
                speed = 0f;
                damage = 0;
                isActivated = false;
                parentWeapon.isAttacking = false;
                Destroy(gameObject);
            }
        }
    }
}
