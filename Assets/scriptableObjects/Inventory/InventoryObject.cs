using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu (fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDataBaseObject database;
    public Inventory Container;
    public int emptySlotCount
    {
        get 
        {
            int count = 0;     
            foreach(InventorySlot slot in Container.Items)
                if(slot.item.ID < 0)
                    count++;
            return count;
        }
    }
    
    public bool AddItem(Item item, int amount)
    {

        if(emptySlotCount <= 0)
            return false;
        InventorySlot slot = FindInInventory(item);
        if(!database.GetItem[item.ID].stackable || slot == null)
        {
            SetItemInEmptySlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
        

        /*
        bool found = false;
        // Проверяем, есть ли предмет в инвентаре
        for (int i = 0; i < Container.Items.Length; i++)
        {
            // Если предмет найден
            if (Container.Items[i].item.ID == item.ID)
            {
                // Проверяем совпадение по количеству баффов
                if (item.buffs.Length == Container.Items[i].item.buffs.Length)
                {   
                    bool buffsMatch = true;

                    // Проверяем совпадение значений баффов
                    for (int j = 0; j < item.buffs.Length; j++)
                    {
                        if (item.buffs[j] != Container.Items[i].item.buffs[j])
                        {
                            buffsMatch = false;
                            break;
                        }
                    }

                        // Если баффы совпадают, увеличиваем количество
                    if (buffsMatch)
                    {
                        Container.Items[i].AddAmount(amount);
                        found = true;
                return;

                    }
                }
            }

        }
        
        if(!found)
        {
            // Если предмета нет, пытаемся поместить его в пустой слот
            SetItemInEmptySlot(item, amount);
        }*/
    }
    
    public InventorySlot SetItemInEmptySlot(Item _item, int _amount)

    {
        for(int i = 0; i < Container.Items.Length; i++)
        {   
            if(Container.Items[i].item.ID <= -1) // Пустой слот
            {
                Container.Items[i].UpdateSlot( _item, _amount);
                return Container.Items[i];
            }                               
        }
        return null;
    }

    public InventorySlot FindInInventory(Item item)
    {
        foreach(InventorySlot slot in Container.Items)
            if(slot.item.ID == item.ID)
                return slot;
        return null;
    }                       

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if(item2.CanPlaceInSlot(item1.itemObject) && item1.CanPlaceInSlot(item2.itemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item _item)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(null, 0);
            }
        }
    }

    /*private void OnEnable()
    {
        #if UNITY_EDITOR
        database = (ItemDataBaseObject)AssetDatabase.LoadAssetAtPath("Asset/scriptableObjects/Inventory/ItemDataBase.asset", typeof(ItemDataBaseObject));
        #endif
    }*/

    /*public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }*/
    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        Debug.Log(string.Concat(Application.persistentDataPath, savePath));
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for(int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

    /*public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }*/

    /*public void OnAfterDeserialize()
    {
        for(int i = 0; i < Container.Items.Count; i++)
        {
            Container.Items[i].item = database.GetItem[Container.Items[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {

    }*/
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[32]; //переименовать в slots
    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}
[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public UserInterface parentInventory;
    public ItemType[] allowedItems = new ItemType[0];
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public ItemObject itemObject
    {
        get 
        {
            if(item.ID > -1)
            {
                return parentInventory.inventory.database.GetItem[item.ID];
            }
            return null;
        }
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if(allowedItems.Length <= 0 || !_itemObject || _itemObject.data.ID < 0)       
            return true;
        for(int i = 0; i < allowedItems.Length; i++)
        {
            if(_itemObject.itemType == allowedItems[i])            
                return true;        
        }
        return false;
    }

    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }
}
