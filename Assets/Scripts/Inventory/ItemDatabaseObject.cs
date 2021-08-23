using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDatabaseObject", menuName = "Inventory System/ItemDatabase")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    public Dictionary<ItemObject, int> GetIdByItem = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItemById = new Dictionary<int, ItemObject>();


    public void OnAfterDeserialize()
    {
        GetIdByItem = new Dictionary<ItemObject, int>();
        GetItemById = new Dictionary<int, ItemObject>();
        
        for (int i = 0; i < Items.Length; i++)
        {
            GetIdByItem.Add(Items[i], i);
            GetItemById.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}