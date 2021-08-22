using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemObject addingItem, int addingAmount)
    {
        bool hasItem = false;
        
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == addingItem)
            {
                Container[i].AddAmount(addingAmount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Container.Add(new InventorySlot(addingItem,addingAmount));
        }
    }
}
