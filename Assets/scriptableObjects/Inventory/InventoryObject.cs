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
    
    public void AddItem(Item item, int amount)
    {
        bool found = false;
        // Проверяем, есть ли предмет в инвентаре
        for (int i = 0; i < Container.Items.Length; i++)
        {
            // Если предмет найден
            if (Container.Items[i].ID == item.ID)
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
        }
    }
    
    public InventorySlot SetItemInEmptySlot(Item _item, int _amount)

    {
        for(int i = 0; i < Container.Items.Length; i++)
        {   
            if(Container.Items[i].ID <= -1) // Пустой слот
            {
                Container.Items[i].UpdateSlot(_item.ID, _item, _amount);
                return Container.Items[i];
            }                               
        }
        return null;
    }                       

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item _item)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
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
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
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
    public InventorySlot[] Items = new InventorySlot[32];
}
[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;
    public UserInterface parentInventory;
    public ItemType[] allowedItems = new ItemType[0];
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public bool CanPlaceInSlot(ItemObject _item)
    {
        if(allowedItems.Length <= 0)
        {
            return true;
        }
        for(int i = 0, count = allowedItems.Length; i < count; i++)
        {
            if(_item.itemType == allowedItems[i])
            {
                return true;
            }
        }
        return false;
    }

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
}
