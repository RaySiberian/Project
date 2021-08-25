using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;

    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i].data.Id != i)
            {
                ItemObjects[i].data.Id = i;
            }
        }
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }
}
