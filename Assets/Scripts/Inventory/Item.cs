[System.Serializable]
public class Item
{
    public int ID;
    public string Name;
    public int Amount;
    
    public Item(ItemObject itemObject)
    {
        ID = itemObject.Data.ID;
        Name = itemObject.name;
        Amount = 1;
    }

    public Item(int id, string name, int amount)
    {
        ID = id;
        Name = name;
        Amount = amount;
    }
    
    public Item()
    {
        ID = -1;
        Name = "";
        Amount = 0;
    }

    public static bool IsEmpty(Item item)
    {
        return item.ID == -1;
    }
}
