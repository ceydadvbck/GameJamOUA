using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Upgrade : ScriptableObject
{
    public string Name;
    public UpgradeType upgradeType;
    public int value;

    public void Equip(Player Player)
    {
        if(upgradeType == UpgradeType.PlayerMaxHealth)
        {
            Player.AddMaxHealth(value);
        }
        else if(upgradeType == UpgradeType.PlayerMaxArmor)
        {
            Player.AddMaxArmor(value);
        }
        else if(upgradeType == UpgradeType.PlayerDashRange)
        {
            Player.AddDashRange(value);
        }
        else if(upgradeType == UpgradeType.PlayerDashDamage)
        {
            Player.AddDashDamage(value);
        }
        else if(upgradeType == UpgradeType.PlayerDashCooldown)
        {
            Player.IncreaseDashCooldown(value);
        }
        else if(upgradeType == UpgradeType.WeaponDamage)
        {
            Player.AddWeaponDamage(value);
        }
        else if(upgradeType == UpgradeType.WeaponAttackSpeed)
        {
            Player.AddWeaponAttackSpeed(value);
        }
        else if(upgradeType == UpgradeType.WeaponRange)
        {
            Player.AddWeaponRange(value);
        }
        else if(upgradeType == UpgradeType.WeaponProjectileSpeed)
        {
            Player.AddWeaponProjectileSpeed(value);
        }
    }
    public void UnEquip(Player Player)
    {
        if (upgradeType == UpgradeType.PlayerMaxHealth)
        {
            Player.RemoveMaxHealth(value);
        }
        else if (upgradeType == UpgradeType.PlayerMaxArmor)
        {
            Player.RemoveMaxArmor(value);
        }
        else if (upgradeType == UpgradeType.PlayerDashRange)
        {
            Player.RemoveDashRange(value);
        }
        else if (upgradeType == UpgradeType.PlayerDashDamage)
        {
            Player.RemoveDashDamage(value);
        }
        else if (upgradeType == UpgradeType.PlayerDashCooldown)
        {
            Player.IncreaseDashCooldown(-value);
        }
        else if (upgradeType == UpgradeType.WeaponDamage)
        {
            Player.RemoveWeaponDamage(value);
        }
        else if (upgradeType == UpgradeType.WeaponAttackSpeed)
        {
            Player.RemoveWeaponAttackSpeed(value);
        }
        else if (upgradeType == UpgradeType.WeaponRange)
        {
            Player.RemoveWeaponRange(value);
        }
        else if (upgradeType == UpgradeType.WeaponProjectileSpeed)
        {
            Player.RemoveWeaponProjectileSpeed(value);
        }
    }
}

