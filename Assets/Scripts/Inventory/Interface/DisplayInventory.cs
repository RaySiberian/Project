using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public InventoryObject inventory;
    [SerializeField] private int xStart;
    [SerializeField] private int yStart;

    [SerializeField] private int xSpaceBetweenItem;
    [SerializeField] private int ySpaceBetweenItems;

    [SerializeField] private int numberOfColumn;

    [SerializeField] private GameObject inventoryPrefab;

    private Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateSlots();
    }

    private void Update()
    {
        //TODO Это не должно быть не каждый кадр
        UpdateSlots();
    }

    private void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                    inventory.dataBase.GetItem[slot.Value.item.Id].sprite;
               
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                    slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
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

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    private void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    
    private void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
        
    }
    
    private void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        
        if (itemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObject.AddComponent<Image>();
            image.sprite = inventory.dataBase.GetItem[itemsDisplayed[obj].ID].sprite;
            image.raycastTarget = false;
        }

        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    
    private void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj],itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    private void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    
    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItem * (i % numberOfColumn)),
            (yStart + (-ySpaceBetweenItems * (i / numberOfColumn))), 0f);
    }

    //TODO На подписку ивентов обновления
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
}