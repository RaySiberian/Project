using UnityEngine;

public class Player : MonoBehaviour
{

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
   
   //TODO Заменить на нормальное условие
   private void OnTriggerEnter(Collider other)
   {
      Debug.Log("3ashel");
       var item = other.GetComponent<ItemInScene>();
       if (item)
       {
           Inventory.AddItem(new Item(item.item),1);
           Destroy(other.gameObject);
       }
   }
}
