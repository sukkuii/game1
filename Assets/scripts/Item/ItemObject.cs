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
        buffs = new ItemBuff[item.buffs.Length];

        for (int i = 0; i < item.buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);// у нас есть конструктор с 3 параметрами
            buffs[i].attribute = item.buffs[i].attribute;
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
