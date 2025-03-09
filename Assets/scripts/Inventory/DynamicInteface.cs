using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInteface : UserInterface
{
    public GameObject inventoryPrefab;
    public int xbetweenItems;
    public int ybetweenItems;
    public int amountOfColumns;
    public int xStart;
    public int yStart;

    public override void CreateSlots()
    {       
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for(int i = 0; i < inventory.Container.Items.Length; i++)  
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            AddEvent(obj, EventTriggerType.PointerEnter, delegate{OnEnter(obj);});
            AddEvent(obj, EventTriggerType.PointerExit, delegate{OnExit(obj);});
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{OnDragStart(obj);});
            AddEvent(obj, EventTriggerType.EndDrag, delegate{OnDragEnd(obj);});
            AddEvent(obj, EventTriggerType.Drag, delegate{OnDrag(obj);});
            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }           
    }

    private Vector3 GetPosition(int i)
    {
        float prefabWidth = inventoryPrefab.GetComponent<RectTransform>().rect.width;// префаб квадратный
        return new Vector3(xStart + (xbetweenItems + prefabWidth) * (i % amountOfColumns), yStart - (ybetweenItems + prefabWidth) * (i / amountOfColumns), 0f);
    }
}
