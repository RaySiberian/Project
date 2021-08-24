using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ISerializationCallbackReceiver нужен для сериалиции SO
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    //public Dictionary<int, ItemObject> GetItem = new Dictionary<int,ItemObject>();

    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].data.Id != i)
            {
                Items[i].data.Id = i;
            }
        }
    }
    
    public void OnBeforeSerialize()
    {
        //GetItem = new Dictionary<int, ItemObject>();
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }
}
