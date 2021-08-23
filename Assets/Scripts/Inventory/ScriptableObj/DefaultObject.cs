using UnityEngine;

[CreateAssetMenu(fileName = "New Default", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Default;
    }
}
