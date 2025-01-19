using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public abstract class ItemObject : ScriptableObject //Abstract что это
{
   public Sprite uiDisplay;
   public ItemType itemType;
   public string description;
   public int ID;
   
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;

    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.ID;
    }
}
