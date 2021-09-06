using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Container inventory;
    private Camera mainCamera;
    [SerializeField] private GameObject InventoryPanels;

    // private GameObject rightHandItem;
    // private GameObject leftHandItem;
    // public Transform rightHandTransform;
    // public Transform leftHandShieldTransform;

    public float MineCastRange = 5f;
    public float MineMultiply = 1f;
    public float AttackRange = 5f;
    public float Attack = 1f;

    private float mineCastRangeBuff = 0f;
    private float mineMultiplyBuff = 0f;
    private float attackRangeBuff = 0f;
    private float attackBuff = 0f;

    private void OnEnable()
    {
        inventory.ContainerUpdated += UpdateBuffsData;
    }

    private void OnDisable()
    {
        inventory.ContainerUpdated -= UpdateBuffsData;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        inventory = GetComponent<Container>();
    }

    //TODO можно улучшить удаляя конкретный баф, не проходясь через весь массив
    private void UpdateBuffsData()
    {
        ClearBuffs();
        int count = 0;
        
        foreach (var equipItem in inventory.Equipment)
        {
            if (equipItem.ID == -1)
            {
                count++;
                if (count == inventory.Equipment.Length)
                {
                    ClearBuffs();
                }
                continue;
            }

            ItemObject itemObject = inventory.FindObjectInDatabase(equipItem);
            EquipmentObject equipment = (EquipmentObject)itemObject;
            SetBuffs(equipment);
        }
    }

    private void SetBuffs(EquipmentObject equipmentObject)
    {
        mineCastRangeBuff = equipmentObject.Buff.MineRangeBonus;
        mineMultiplyBuff = equipmentObject.Buff.MineMultiplyBonus;
        attackRangeBuff = equipmentObject.Buff.AttackRangeBonus;
        attackBuff = equipmentObject.Buff.AttackBonus;
        ApplyBuffs();
    }
    
    private void ClearBuffs()
    {
        mineCastRangeBuff = 0f;
        mineMultiplyBuff = 0f;
        attackRangeBuff = 0f;
        attackBuff = 0f;
        
        MineCastRange = 5f;
        MineMultiply = 1f;
        AttackRange = 5f;
        Attack = 1f;
    }
    
    private void ApplyBuffs()
    {
        MineCastRange += mineCastRangeBuff;
        MineMultiply += mineMultiplyBuff;
        AttackRange += attackRangeBuff;
        Attack += attackBuff;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerHit();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateBuffsData();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            MineCastRange = 5;
        }
    }

    private void HideInventory()
    {
        //TODO баг с инвенторем при отключении
        InventoryPanels.SetActive(!InventoryPanels.activeSelf);
    }

    private void PlayerHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, MineCastRange))
        {
            if (hit.collider.CompareTag("Resources"))
            {
                WorldItem worldItem = hit.collider.gameObject.GetComponent<WorldItem>();
                Item item = new Item(worldItem.ItemId, worldItem.Name, worldItem.GiveResources());
                inventory.AddItemInInventory(item);
            }
        }
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     var item = other.GetComponent<GroundItem>();
    //     if (item)
    //     {
    //         Item _item = new Item(item.item);
    //         if (Inventory.AddItem(_item, 1))
    //         {
    //             Destroy(other.gameObject);
    //         }
    //     }
    // }
}