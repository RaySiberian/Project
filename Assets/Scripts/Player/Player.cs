using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject Inventory;
    public InventoryObject Equipment;
    public Attribute[] Attributes;

    private GameObject rightHandItem ;
    private GameObject leftHandItem ;
    
    public Transform rightHandTransform;
    public Transform leftHandShieldTransform;
     
    private void Start()
    {
        for (int i = 0; i < Attributes.Length; i++)
        {
            Attributes[i].SetParent(this);
        }

        for (int i = 0; i < Equipment.GetSlots.Length; i++)
        {
            Equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            Equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
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

    public void OnRemoveItem(InventorySlot slot)
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

                if (slot.ItemObject.type == ItemType.Weapon && rightHandItem != null)
                {
                    Destroy(rightHandItem);
                }
                
                if (slot.ItemObject.type == ItemType.Shield && leftHandItem != null)
                {
                    Destroy(leftHandItem);
                }
                
                break;
            case InterfaceType.Chest:
                break;
        }
    }

    public void OnAddItem(InventorySlot slot)
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

                if (slot.ItemObject.characterDisplay != null)
                {
                    Debug.Log(slot.slotDisplay.name);
                    if (slot.ItemObject.type == ItemType.Shield)
                    { 
                        leftHandItem = Instantiate(slot.ItemObject.characterDisplay, leftHandShieldTransform);
                    }

                    if (slot.ItemObject.type == ItemType.Weapon)
                    {
                        rightHandItem = Instantiate(slot.ItemObject.characterDisplay, rightHandTransform);
                    }
                }
                
                break;
            case InterfaceType.Chest:
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
        //Debug.Log(string.Concat(attribute.type, "was updated! Value is now", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        Inventory.Clear();
        Equipment.Clear();
    }
}