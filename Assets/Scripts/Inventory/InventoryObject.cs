using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDataBaseObject dataBase;
    public Inventory Container;
    public string savePath;

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.Id <= 1)
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
        
        if (!dataBase.GetItem[item.Id].stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }
        
        slot.AddAmount(amount);
        return true;
    }

    public InventorySlot FindItemInInventory(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id == item.Id)
            {
                return Container.Items[i];
            }
        }

        return null;
    }
    
    private InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id<= -1)
            {
                Container.Items[i].UpdateSlot(item, amount);
                return Container.Items[i];
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
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSlot(null, 0);
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

            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item,
                    newContainer.Items[i].amount);
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
    public InventorySlot[] Items = new InventorySlot[56];

    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    [NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                return parent.inventory.dataBase.GetItem[item.Id];
            }

            return null;
        }
    }

    public void UpdateSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
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