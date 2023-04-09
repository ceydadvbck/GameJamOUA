using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    [SerializeField] List<Item> items;
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {

    }

    public void Equip(Item itemToEquip)
    {
        if (items == null)
        {
            items = new List<Item>();
        }
        items.Add(itemToEquip);
        itemToEquip.Equip(player);
    }

    public void unEquip(Item itemToUnEquip)
    {

    }
}
