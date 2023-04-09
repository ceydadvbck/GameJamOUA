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
    public GameObject[] weapons; /*Bu silahın animasyonu için child objeleri tutan değişken. Silahın animasyonu için 4 adet child obje oluşturulacak. Bunlar up, down, left, right olacak. Sırası aynen böyle olmalı.
    Bu nesneleri parent içine alıp pivot değişikliği yapacağız bu sayede nesnenin scale edilme yönünü kontrol edeceğiz.*/
    private float lastAttackTime; //Son atak yapıldığı zamanı tutan değişken. Bu sayede belli bir zaman aralığı ile saldırı yapılmasını sağlayabiliriz.
    private Player player; //Bu player nesnesi, singleton yapıda olduğundan direkt Player.Instance ile çağırabiliriz.
    public bool isActivated, isAttacking; /*Bunlar silahın aktif olup olmadığını ve o anda saldırı durumunda olup olmadığını kontrol etmek için.
                                            Silahın saldırı animasyonu bitince isAttacking false olacak ve yeni bir saldırı yapılabilir.*/

    public void Start()
    {
        player = Player.Instance;
        if (weaponType == WeaponType.Melee)
        {
            weapons = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                weapons[i] = transform.GetChild(i).gameObject;
                weapons[i].transform.localScale = Vector3.zero;
                weapons[i].SetActive(false);
            }
        }
        player.currentWeapon = weaponType;
    }

    public void Update()
    {
        if (isActivated && !isAttacking)
        {
            if (Time.time - lastAttackTime >= attackSpeed)
            {
                isAttacking = true;
                Attack(player.lastAttackDirection);
            }
        }
    }

    public void Attack(Direction attackDirection)
    {
        if (weaponType == WeaponType.Melee)
        {
            int index = (int)attackDirection;
            weapons[index].SetActive(true);
            weapons[index].transform.DOScale(Vector3.one, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
            {
                weapons[index].transform.DOScale(Vector3.zero, animationDuration * 0.5f).SetEase(attackEase).OnComplete(() =>
                {
                    isAttacking = false;
                    lastAttackTime = Time.time;
                    weapons[index].SetActive(false);
                });
            });

        }
        else if (weaponType == WeaponType.Ranged)
        {
            if (weapons.Length < 4)
                return;

            if (attackDirection == Direction.Up)
            {
                GameObject.Instantiate(weapons[0], transform.position, Quaternion.identity).GetComponent<RangedWeapon>().SetNewAttack(this, Vector2.up, projectileSpeed, damage, range);
            }
            else if (attackDirection == Direction.Down)
            {
                GameObject.Instantiate(weapons[1], transform.position, Quaternion.identity).GetComponent<RangedWeapon>().SetNewAttack(this, Vector2.down, projectileSpeed, damage, range);
            }
            else if (attackDirection == Direction.Left)
            {
                GameObject.Instantiate(weapons[2], transform.position, Quaternion.identity).GetComponent<RangedWeapon>().SetNewAttack(this, Vector2.left, projectileSpeed, damage, range);
            }
            else if (attackDirection == Direction.Right)
            {
                GameObject.Instantiate(weapons[3], transform.position, Quaternion.identity).GetComponent<RangedWeapon>().SetNewAttack(this, Vector2.right, projectileSpeed, damage, range);
            }
            lastAttackTime = Time.time;
            isAttacking = false;
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
