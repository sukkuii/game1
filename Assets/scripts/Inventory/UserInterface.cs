using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public PlayerInventory player;
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    void Start()
    {
        for(int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parentInventory = this;
        }
        CreateSlots();
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void UpdateSlots()
    {
        foreach(KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if(slot.Value.ID > -1)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.item.ID].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }  
        }
    }

    void Update()
    {
        UpdateSlots();
    }

    /*public void CreateDisplay()
    {
        for(int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].uiDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(slot, obj);
        }
    }*/

    public abstract void CreateSlots(); 

    public void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;
        if(itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObj = new GameObject();
        var rt = mouseObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObj.transform.SetParent(transform.parent);
        if(itemsDisplayed[obj].ID > -1)
        {
            var image = mouseObj.AddComponent<Image>();
            image.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            image.raycastTarget = false;
        }
        player.mouseItem.obj = mouseObj;
        player.mouseItem.slot = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)// проверка чтобы пустой слот нельзя было положить
    {
        var itemInMouse = player.mouseItem;
        var mouseHoverItem = itemInMouse.hoverItem;
        var mouseHoverObj = itemInMouse.hoverObj;
        var getItemObject = inventory.database.GetItem;
        if(mouseHoverObj)
        {
            if(mouseHoverItem.CanPlaceInSlot(getItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.ID <= -1) 
            || ((mouseHoverItem.item.ID >= 0) && (itemsDisplayed[obj].CanPlaceInSlot(getItemObject[mouseHoverItem.item.ID]))))

                inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parentInventory.itemsDisplayed[mouseHoverObj]);
        }
        else
        {
            //inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy (itemInMouse.obj);
        itemInMouse.slot = null;
    }

    public void OnDrag(GameObject obj)
    {
        if(player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;  
        }
    }

    

    

    /*public void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            if(itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].uiDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                itemsDisplayed.Add(slot, obj);
            }
        }
    }*/
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot slot;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
