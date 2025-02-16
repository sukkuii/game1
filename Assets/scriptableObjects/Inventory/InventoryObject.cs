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
    
    public void AddItem(Item item, int amount)// сделать так чтобы предметы с однинаковым числовым значением бафафов стакались а с разным числовым значением баффов не стакались. проверить одинаковые баффы
    {
        if(item.buffs.Length > 0)
        {
            var item1 = SetItemInEmptySlot(item, amount);   
            return;
        }

        for(int i = 0; i < Container.Items.Length; i++)
        {
            Debug.Log(Container.Items[i].item.ID);
            Debug.Log(item.ID);
            if(Container.Items[i].item.ID == item.ID)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        } 
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item.ID == item.ID)
            {
                if(item.buffs.Length == Container.Items[i].item.buffs.Length)
                {
                    for(int j = 0; j < item.buffs.Length; i++)
                    {
                        if(item.buffs[j] != Container.Items[i].item.buffs[j])
                        {
                            break;
                        }
                    }
                }
            }
        }
    }



    public InventorySlot SetItemInEmptySlot(Item _item, int _amount)
    {
        for(int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.ID, _item, _amount);
                return  Container.Items[i];
            }
        }
        return null;// сюда нужно вернуться дописать если инвентарь полный
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
            Container = (Inventory)formatter.Deserialize(stream);
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
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
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



