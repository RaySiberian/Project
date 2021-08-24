using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ItemType
{
    Food,
    Helmet,
    Weapon,
    Shield,
    Boots,
    Chest,
    Default 
}

public enum Attributes
{
    Agility = 0,
    Intellect = 1,
    Stamina = 2,
    Strength = 3
}

public abstract class ItemObject : ScriptableObject
{
    [TextArea(15, 20)] public string description;
    public Sprite sprite;
    public ItemType type;
    public int Id;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[Serializable]
public class Item
{
    public string Name;
    public int Id;
    public ItemBuff[] buffs;

    public Item()
    {
        Name = "";
        Id = -1;
    }
    
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            buffs[i].attribute = item.buffs[i].attribute;
        }
    }
}

[Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int min, int max)
    {
        this.max = max;
        this.min = min;
        GenerateValue();
    }

    private void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}