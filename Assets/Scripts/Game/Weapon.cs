using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    /*Bu sınıf silahlar ile alakalı verilerin tutulduğu sınıf. Silah aktif olduğu sürece kendi scripti üzerinden attack işlemini gerçekleştirecek.
    silahlar sadece playerda olduğu için ayrı bir controllera gerek duymadım her silah kendini kontrol etsin biz sadece aç kapat aktif deaktif yapalım.*/
    public WeaponType weaponType; //Silah tiplerinin tanımlandığı kısım, bu sayede Attack fonksiyonunda silaha uygun şekilde animasyon yapabiliriz.
    public int damage;
    public float attackSpeed;
    public float range;
    public float projectileSpeed;
    public Ease attackEase; //Silahın animasyonu için kullanılacak ease.
    public float animationDuration; //Silahın animasyonu için kullanılacak süre. Silahın görünmesi ve kaybolması toplamı bu süre olacak.
    private Transform[] weapons; /*Bu silahın animasyonu için child objeleri tutan değişken. Silahın animasyonu için 4 adet child obje oluşturulacak. Bunlar up, down, left, right olacak. Sırası aynen böyle olmalı.
    Bu nesneleri parent içine alıp pivot değişikliği yapacağız bu sayede nesnenin scale edilme yönünü kontrol edeceğiz.*/
    private float lastAttackTime; //Son atak yapıldığı zamanı tutan değişken. Bu sayede belli bir zaman aralığı ile saldırı yapılmasını sağlayabiliriz.
    private Player player; //Bu player nesnesi, singleton yapıda olduğundan direkt Player.Instance ile çağırabiliriz.
    public bool isActivated, isAttacking; /*Bunlar silahın aktif olup olmadığını ve o anda saldırı durumunda olup olmadığını kontrol etmek için.
                                            Silahın saldırı animasyonu bitince isAttacking false olacak ve yeni bir saldırı yapılabilir.*/

    public void Start()
    {
        player = Player.Instance;
        weapons = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
            weapons[i].localScale = Vector3.zero;
        }
    }

    public void Update()
    {
        if (isActivated && !isAttacking)
        {
            if (Time.time - lastAttackTime >= attackSpeed)
            {
                isAttacking = true;
                Attack(player.lastDirection);
            }
        }
    }

    public void Attack(AttackDirection attackDirection)
    {
        if (attackDirection == AttackDirection.Up)
        {
            weapons[0].DOScale(Vector3.one, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
            {
                weapons[0].DOScale(Vector3.zero, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
                {
                    isAttacking = false;
                    lastAttackTime = Time.time;
                });
            });
        }
        else if (attackDirection == AttackDirection.Down)
        {
            weapons[1].DOScale(Vector3.one, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
            {
                weapons[1].DOScale(Vector3.zero, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
                {
                    isAttacking = false;
                    lastAttackTime = Time.time;
                });
            });
        }
        else if (attackDirection == AttackDirection.Left)
        {
            weapons[2].DOScale(Vector3.one, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
            {
                weapons[2].DOScale(Vector3.zero, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
                {
                    isAttacking = false;
                    lastAttackTime = Time.time;
                });
            });
        }
        else if (attackDirection == AttackDirection.Right)
        {
            weapons[3].DOScale(Vector3.one, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
            {
                weapons[3].DOScale(Vector3.zero, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
                {
                    isAttacking = false;
                    lastAttackTime = Time.time;
                });
            });
        }
    }

    public void Upgrade(WeaponUpgrade weaponUpgrade)
    {
        if (weaponUpgrade.weaponUpgradeType == WeaponUpgradeType.Damage)
        {
            damage += (int)weaponUpgrade.value;
        }
        else if (weaponUpgrade.weaponUpgradeType == WeaponUpgradeType.AttackSpeed)
        {
            attackSpeed += weaponUpgrade.value;
        }
        else if (weaponUpgrade.weaponUpgradeType == WeaponUpgradeType.Range)
        {
            range += weaponUpgrade.value;
        }
        else if (weaponUpgrade.weaponUpgradeType == WeaponUpgradeType.ProjectileSpeed)
        {
            projectileSpeed += weaponUpgrade.value;
        }
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Deactivate()
    {
        isActivated = false;
    }
}
