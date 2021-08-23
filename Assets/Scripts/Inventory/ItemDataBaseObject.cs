using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ISerializationCallbackReceiver нужен для сериалиции SO
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int,ItemObject>();
    
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].Id = i;
            GetItem.Add(i,Items[i]);
        }
    }
}
