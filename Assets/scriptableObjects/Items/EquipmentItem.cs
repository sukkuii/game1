using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]

public class EquipmentItem : ItemObject
{
    public int attackBonus;
    public int defenceBonus;
    public void Awake()
    {
        itemType = ItemType.Equipment;
    }
}
