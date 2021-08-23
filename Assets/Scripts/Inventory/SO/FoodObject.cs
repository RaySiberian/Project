using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int restoreHealth;

    private void Awake()
    {
        type = ItemType.Food;
    }
}
