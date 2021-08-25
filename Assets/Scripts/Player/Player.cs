using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject Inventory;
    public InventoryObject Equipment;
    public Attribute[] Attributes;

    private void Start()
    {
        for (int i = 0; i < Attributes.Length; i++)
        {
            Attributes[i].SetParent(this);
        }

        for (int i = 0; i < Equipment.GetSlots.Length; i++)
        {
            Equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            Equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Inventory.Load();
            Equipment.Load();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Inventory.Save();
            Equipment.Save();
        }
    }
    /// <summary>
    /// OnBeforeUpdate вызывается после удаления предмета из слота
    /// </summary>
    public void OnBeforeSlotUpdate(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }
        
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                Debug.Log("Removed " + slot.ItemObject.name + " On " + slot.parent.inventory.type);
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].type == slot.item.buffs[i].attribute)
                        {
                            Attributes[j].value.RemoveModifier(slot.item.buffs[i]);
                        }
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// OnAfterUpdate вызывается при добавлении предмета в слот
    /// </summary>
    public void OnAfterSlotUpdate(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }
        
        switch (slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                //Почему-то дебаг кидает нулу
                Debug.Log("Added " + slot.ItemObject.name);
                for (int i = 0; i < slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < Attributes.Length; j++)
                    {
                        if (Attributes[j].type == slot.item.buffs[i].attribute)
                        {
                            Attributes[j].value.AddModifier(slot.item.buffs[i]);
                        }
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            if (Inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "was updated! Value is now", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        Inventory.Clear();
        Equipment.Clear();
    }
}