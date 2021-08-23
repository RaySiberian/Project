using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject Database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
        //TODO из-за этого игра не забилдится 
        Database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("InventoryAssest/Database.asset",
            typeof(ItemDatabaseObject));
    }

    public void AddItem(ItemObject addingItem, int addingAmount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == addingItem)
            {
                Container[i].AddAmount(addingAmount);
                return;
            }
        }

        //TODO разобраться с Database.GetId[addingItem]
        Container.Add(new InventorySlot(Database.GetIdByItem[addingItem], addingItem, addingAmount));
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = Database.GetItemById[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}