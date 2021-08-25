using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDataBaseObject dataBase;
    public Inventory Container;
    public InterfaceType type;
    public InventorySlot[] GetSlots => Container.Slots;
    public string savePath;

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= 1)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        InventorySlot slot = FindItemInInventory(item);
        
        if (EmptySlotCount <= 0)
        {
            return false;
        }
        
        if (!dataBase.ItemObjects[item.Id].stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }
        
        slot.AddAmount(amount);
        return true;
    }

    public InventorySlot FindItemInInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }

        return null;
    }
    
    private InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i <GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id<= -1)
            {
                GetSlots[i].UpdateSlot(item, amount);
                return GetSlots[i];
            }
        }

        //TODO обработка полного инвентаря
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot tepm = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(tepm.item, tepm.amount);
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create,
            FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }


    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open,
                FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item,
                    newContainer.Slots[i].amount);
            }

            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[Serializable]
public class Inventory
{
    // В скрипте Player задается тоже 
    public InventorySlot[] Slots = new InventorySlot[56];

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot slot);

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    [NonSerialized] public UserInterface parent;
    [NonSerialized] public GameObject slotDisplay;
  
    [NonSerialized] public SlotUpdated OnAfterUpdate;
 
    [NonSerialized] public SlotUpdated OnBeforeUpdate;
    public Item item;
    public int amount;
 
    public ItemObject ItemObject => item.Id >= 0 ? parent.inventory.dataBase.ItemObjects[item.Id] : null;

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
      
    }

    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }
    
    public void UpdateSlot(Item item, int amount)
    {
        OnBeforeUpdate?.Invoke(this);
        
        this.item = item;
        this.amount = amount;
        
        OnAfterUpdate?.Invoke(this);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(),0);
    }
    
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (allowedItems.Length <= 0  || itemObject == null || itemObject.data.Id < 0)
        {
            return true;
        }

        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (itemObject.type == allowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}