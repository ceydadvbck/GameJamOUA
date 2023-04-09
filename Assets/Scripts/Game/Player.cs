using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoSingleton<Player>
{
    //Bu scriptte playerın özelliklerini tutuyoruz. Playerın özelliklerini etkileyen her fonksiyon burada olacak. Buradan çağırılacak.
    public float moveSpeed;
    public int maxHealth;
    public int currentHealth;
    public int maxArmor;
    public int currentArmor;
    public int dashAmount;
    public float dashDistance;
    public bool dashCooldown;
    public float dashCooldownTime;
    public int dashDamage;
    public int xpAmount;
    public int maxXP;
    public int specialRange;
    public int specialDamage;
    public float specialKnockback;
    public GameObject dashEffect;
    public WeaponType currentWeapon;
    public Direction lastDirection; //Karakterin yöneldiği son yönü PlayerController'ın Move fonksiyonu ile veriyoruz.
    public Direction lastAttackDirection; //Karakterin son saldırı yönü. Attack fonksiyonu içinde kullanılıyor.

    public void AddHealth(int amount)
    {
        int tempHealth = currentHealth;
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            if (tempHealth != maxHealth)
            {
                GameUIController.Instance.PushMessage("Health Full!");
                GameUIController.Instance.healthMaxedOut();
            }
        }
    }

    public void RemoveHealth(int amount)
    {
        AddArmor(ref amount); //S (Passive item sistemi için eklenenler.)
        int tempHealth = currentHealth;
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            if (tempHealth != 0)
            {
                GameUIController.Instance.PushMessage("You Died!");
                //GameManager.Instance.GameOver();
            }
        }
    }

    public void AddArmor(ref int amount) //S (ref ekledim)
    {
        amount -= currentArmor; //S
        if (amount < 0) { amount = 0; } //S
        int tempArmor = currentArmor;
        currentArmor += amount;
        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
            if (tempArmor != maxArmor)
            {
                GameUIController.Instance.PushMessage("Armor Full!");
                GameUIController.Instance.armorMaxedOut();
            }
        }
    }

    public void RemoveArmor(int amount)
    {
        currentArmor -= amount;
        if (currentArmor < 0)
        {
            currentArmor = 0;
        }
    }

    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
    }

    public void RemoveMaxHealth(int amount)
    {
        maxHealth -= amount;
    }

    public void AddMaxArmor(int amount)
    {
        maxArmor += amount;
    }

    public void RemoveMaxArmor(int amount)
    {
        maxArmor -= amount;
    }

    public void AddDashRange(int amount)
    {
        dashDistance += amount;
    }

    public void RemoveDashRange(int amount)
    {
        dashDistance -= amount;
    }

    public void AddDashDamage(int amount)
    {
        dashDamage += amount;
    }

    public void RemoveDashDamage(int amount)
    {
        dashDamage -= amount;
    }

    public void IncreaseDashCooldown(int amount)
    {
        dashCooldownTime -= amount;
    }

    public void DecreaseDashCooldown(int amount)
    {
        dashCooldownTime += amount;
    }

    public void AddXP(int amount)
    {
        int tempXP = xpAmount;
        xpAmount += amount;
        if (xpAmount >= maxXP)
        {
            xpAmount = maxXP;
            if (tempXP != maxXP)
            {
                GameUIController.Instance.PushMessage("Special Bar Full!");
                GameUIController.Instance.xpMaxedOut();
            }
        }
    }

    public void AddWeaponDamage(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.damage += amount;
        }
    }

    public void RemoveWeaponDamage(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.damage -= amount;
        }
    }

    public void AddWeaponAttackSpeed(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.attackSpeed += amount;
        }
    }

    public void RemoveWeaponAttackSpeed(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.attackSpeed -= amount;
        }
    }

    public void AddWeaponRange(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.range += amount;
        }
    }

    public void RemoveWeaponRange(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.range -= amount;
        }
    }

    public void AddWeaponProjectileSpeed(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.projectileSpeed += amount;
        }
    }

    public void RemoveWeaponProjectileSpeed(int amount)
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.projectileSpeed -= amount;
        }
    }
}
