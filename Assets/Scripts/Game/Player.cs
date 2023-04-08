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
    public int coin;
    [NonSerialized] public List<Weapon> weapons;
    public AttackDirection lastDirection; //Karakterin yöneldiği son yönü PlayerController'ın Move fonksiyonu ile veriyoruz. Attack yaparken Weapon sınıfı bu değeri direkt çekip yön belirlemek için kullanıyor.

    public void Start()
    {
        weapons = new List<Weapon>();
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        weapons[weapons.Count - 1].transform.SetParent(transform);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public void RemoveCoin(int amount)
    {
        coin -= amount;
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void RemoveHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void AddArmor(int amount)
    {
        currentArmor += amount;
        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
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
}
