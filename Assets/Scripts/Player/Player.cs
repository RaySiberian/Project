using UnityEngine;

public class Player : MonoBehaviour
{
   public InventoryObject Inventory;
   public InventoryObject Equipment;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         Inventory.Load();
         Equipment.Load();
      }
      if (Input.GetKeyDown(KeyCode.K))
      {
         Inventory.Save();
         Equipment.Save();
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      var item = other.GetComponent<GroundItem>();
      if (item)
      {
         Item _item = new Item(item.item);
         if (Inventory.AddItem(_item,1))
         {
            Destroy(other.gameObject);
         }
      }

   }

   private void OnApplicationQuit()
   {
      Inventory.Container.Clear();
      Equipment.Container.Clear();
   }
}
