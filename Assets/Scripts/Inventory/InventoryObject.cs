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

    public void AddItem(Item item, int amount)
    {
        if (item.buffs.Length > 0)
        {
            SetEmptySlot(item, amount);
            return;
        }

        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == item.Id)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        }

        SetEmptySlot(item, amount);
    }

    private InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(item.Id, item, amount);
                return Container.Items[i];
            }
        }

        //TODO обработка полного инвентаря
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot tepm = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(tepm.ID, tepm.item, tepm.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
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
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item,
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
            Items[i].UpdateSlot(-1, new Item(), 0);
        }
    }
}

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    public UserInterface parent;
    public int ID;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int id, Item item, int amount)
    {
        ID = id;
        this.item = item;
        this.amount = amount;
    }

    public void UpdateSlot(int id, Item item, int amount)
    {
        ID = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemObject item)
    {
        if (allowedItems.Length <= 0)
        {
            return true;
        }

        for (int i = 0; i < allowedItems.Length; i++)
        {
            if (item.type == allowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}