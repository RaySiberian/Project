[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    //ID - это порядковый номер объета в массиве Database
    public int ID;
    public int amount;

    public InventorySlot(int ID, ItemObject item, int amount)
    {
        this.ID = ID;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}