using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public Player player;

    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    public abstract void CreateSlots();

    private void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }
    
    private void Update()
    {
        //TODO Это не должно быть не каждый кадр
        UpdateSlots();
    }

    public void UpdateSlots()
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

    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnEnterInterface(GameObject obj)
    {
        player.mouseItem.ui = obj.GetComponent<UserInterface>();
    }
    
    public void OnExitInterface(GameObject obj)
    {
        player.mouseItem.ui = null;
    }
    
    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
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

        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.dataBase.GetItem;

        if (itemOnMouse.ui != null)
        {
            if (mouseHoverObj)
            {
                if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.Id <= -1 ||
                    (mouseHoverItem.item.Id >= 0 && itemsDisplayed[obj].CanPlaceInSlot(GetItemObject[mouseHoverItem.item.Id]))))
                {
                    inventory.MoveItem(itemsDisplayed[obj],
                        mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
                }
            }
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}


public class MouseItem
{
    public UserInterface ui;
    public GameObject obj;
    public GameObject hoverObj;
    public InventorySlot item;
    public InventorySlot hoverItem;
}