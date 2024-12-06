using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public class ItemObject : ScriptableObject
{
   public GameObject prefab;
   public ItemType itemType;
   public string description;
   
}
