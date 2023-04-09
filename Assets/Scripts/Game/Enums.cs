using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bu script oyun içinde bulunan düşman tipi, silah tipi, yükseltme tipleri gibi enumları tutuyor. Tip belirlerken çok işe yarayacak.

public enum EnemyType
{
    Melee,
    Ranged
}

public enum ObjectType
{
    Player,
    Enemy,
    Item,
}

public enum ItemType
{
    Health,
    Weapon,
    Armor,
    Upgrade,
    Coin
}

public enum WeaponType
{
    Melee,
    Ranged
}

public enum WeaponUpgradeType
{
    Damage,
    AttackSpeed,
    Range,
    ProjectileSpeed
}

public enum PlayerUpgradeType
{
    Health,
    MovementSpeed
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}