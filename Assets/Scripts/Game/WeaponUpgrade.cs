using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : ScriptableObject
{
    //Bu scriptable obje silah yükseltmelerini tutacak. Yükseltme tipi ve değer alıyor sadece. Weapon sınıfında Upgrade fonksiyonu içinde kullanılacak.
    public WeaponUpgradeType weaponUpgradeType;
    public float value;
}
