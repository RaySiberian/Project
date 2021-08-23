using System;
using UnityEngine;

public class Player : MonoBehaviour
{
   public InventoryObject Inventory;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         Inventory.Save();
      }
      
      if (Input.GetKeyDown(KeyCode.L))
      {
         Inventory.Load();
      }

   }
}
