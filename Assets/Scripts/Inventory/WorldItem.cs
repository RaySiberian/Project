using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField]private ItemObject worldItemObject;
    public int ItemId;
    public int Amount;
    public string Name;
    
    public int AmountMultiplayerByDurability;
    public int Durability = 5;

    private void Start()
    {
        ItemId = worldItemObject.Data.ID;
        Name = worldItemObject.Data.Name;
        Amount = Durability * AmountMultiplayerByDurability;
    }

    public int GiveResources()
    {
        int returnNumber = Amount / Durability;
        Amount -= returnNumber;
        Durability --;
        if (Durability == 0)
        {
            Destroy(gameObject);
        }
        return returnNumber;
    }
}