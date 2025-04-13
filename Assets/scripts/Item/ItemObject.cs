using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Helmet,
    Chestplate,
    Boots,
    Sword,
    Shield,
    Ring,
    Bracers,
    Default
}

public enum Attributes
{
    Strength,
    Agility,
    Intellect
}

public abstract class ItemObject : ScriptableObject
{
   public Sprite uiDisplay;
   public bool stackable;
   public ItemType itemType;
   public string description;
   public Item data = new Item();

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
    public int ID = -1;
    public ItemBuff[] buffs;
    public ItemType itemType; // Added itemType property
    public Item()
    {
        Name = "";
        ID = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.data.ID;
        itemType = item.itemType; // Set itemType from ItemObject
        buffs = new ItemBuff[item.data.buffs.Length];

        for (int i = 0; i < item.data.buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
}

[System.Serializable]
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
        GenerateValue();
    }

    public ItemBuff(int _min, int _max, Attributes _attribute)
    {
        attribute = _attribute;
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}
