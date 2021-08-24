using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDataBaseObject dataBase;
    public Inventory container;
    public string savePath;

    public void AddItem(Item item, int amount)
    {
        if (item.buffs.Length > 0)
        {
            SetEmptySlot(item, amount);
            return;
        }

        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].ID == item.Id)
            {
                container.items[i].AddAmount(amount);
                return;
            }
        }

        SetEmptySlot(item, amount);
    }

    private InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].ID <= -1)
            {
                container.items[i].UpdateSlot(item.Id, item, amount);
                return container.items[i];
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
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item == item)
            {
                container.items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create,
            FileAccess.Write);
        formatter.Serialize(stream, container);
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

            for (int i = 0; i < container.items.Length; i++)
            {
                container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item,
                    newContainer.items[i].amount);
            }

            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }
}

[Serializable]
public class Inventory
{
    // В скрипте Player задается тоже 
    public InventorySlot[] items = new InventorySlot[56];
}

[Serializable]
public class InventorySlot
{
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
}