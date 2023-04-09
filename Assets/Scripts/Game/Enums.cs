using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bu script oyun içinde bulunan düşman tipi, silah tipi, yükseltme tipleri gibi enumları tutuyor. Tip belirlerken çok işe yarayacak.

public enum EnemyType
{
    Melee,
    Ranged
}

public enum WeaponType
{
    Melee,
    Ranged
}

public enum UpgradeType
{
    WeaponDamage,
    WeaponAttackSpeed,
    WeaponRange,
    WeaponProjectileSpeed,
    PlayerMaxHealth,
    PlayerMaxArmor,
    PlayerDashRange,
    PlayerDashDamage,
    PlayerDashCooldown,
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}