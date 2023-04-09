using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    [SerializeField] List<Upgrade> items;
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Equip(Upgrade itemToEquip)
    {
        if (items == null)
        {
            items = new List<Upgrade>();
        }
        items.Add(itemToEquip);
        itemToEquip.Equip(player);
    }

    public void unEquip(Upgrade itemToUnEquip)
    {
        if (items == null)
        {
            return;
        }
        items.Remove(itemToUnEquip);
        itemToUnEquip.UnEquip(player);
    }
}
