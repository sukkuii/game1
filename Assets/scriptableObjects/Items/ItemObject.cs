using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public enum Attributes
{
    Strength,
    Agility,
    Intellect
}

public abstract class ItemObject : ScriptableObject //Abstract что это
{
   public Sprite uiDisplay;
   public ItemType itemType;
   public string description;
   public int ID;
   public ItemBuff[] buffs;

   public Item CreateItem()
   {
        Item newItem = new Item(this);
        return newItem;
   } 
   
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;
    public ItemBuff[] buffs;

    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.ID;
        buffs = new ItemBuff[item.buffs.Length];// для каждого элемента buffs нужно присвоит значение из item.buffs
    }
}

public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}
