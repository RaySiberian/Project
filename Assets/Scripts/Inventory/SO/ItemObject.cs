using UnityEngine;

public enum ItemType
{
    Default = 0,
    Equipment = 1,
    Food = 2,
}
/// <summary>
/// Общий шаблон объектов
/// </summary>
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
