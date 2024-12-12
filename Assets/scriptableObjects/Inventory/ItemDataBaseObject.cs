using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item Database", menuName = "Inventory System/Items/Database")]

public class ItemDataBaseObject : ScriptsableObject, ISerializationCallbackReceiver;
{
    public ItemObject[] items;
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
