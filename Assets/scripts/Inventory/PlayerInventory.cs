using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if(item)
        {
            if(inventory.AddItem(new Item(item.item), 1))
                Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear(); 
        equipment.Container.Clear();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();         
            equipment.Save(); 
        }
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }
}
