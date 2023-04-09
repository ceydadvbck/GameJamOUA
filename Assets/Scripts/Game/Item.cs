using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Item : ScriptableObject
{
    public string Name;
    public int currentArmor;

    public void Equip(Player Player)
    {
        Player.currentArmor += currentArmor;
    }
    public void UnEquip(Player Player)
    {
        Player.currentArmor -= currentArmor;
    }
}

