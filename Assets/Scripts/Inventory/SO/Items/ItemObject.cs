using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public Item Data = new Item();
    public int MaxStuckSize;
    public bool StackAble;
    public Sprite UISprite;
    public GameObject WorldPrefab;
    public ItemObjectType Type;

    protected virtual void Awake()
    {
       
    }
}

public enum ItemObjectType
{
    Resources = 0,
    Food = 1,
    Equipment = 2
}
