using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item Database", menuName = "Inventory System/Items/Database")]

public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();// стоит ли поправить ID в словаре??
    
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < items.Length; i++)
        {
            items[i].data.ID = i;
            GetItem.Add(i, items[i]);
            
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}
