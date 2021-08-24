using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Chest;
    }
}
