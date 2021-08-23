using UnityEngine;

public class Player : MonoBehaviour
{
   public InventoryObject Inventory;

   private void OnTriggerEnter(Collider other)
   {
      var item = other.GetComponent<GroundItem>();
      if (item)
      {
         Inventory.AddItem(new Item(item.item),1);
      }

   }
}
