using System;
using UnityEngine;

public class Player : MonoBehaviour
{
   public MouseItem mouseItem = new MouseItem();
   public InventoryObject Inventory;

   private void OnTriggerEnter(Collider other)
   {
      var item = other.GetComponent<GroundItem>();
      if (item)
      {
         Inventory.AddItem(new Item(item.item),1);
      }

   }

   private void OnApplicationQuit()
   {
      Inventory.Container.Items = new InventorySlot[56];
   }
}
