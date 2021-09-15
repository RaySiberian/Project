using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action InventoryOpen;
    
    public GameObject WeaponHandlerGameObject;
    
    public float MineCastRange = 5f;
    public float MineMultiply = 1f;
    public float AttackRange = 5f;
    public float Attack = 1f;
    
    public GameObject prefabModel1;
    public GameObject prefabModel2;

    private GameObject currentModel;
    
    private GameObject weapon;
    private Container inventory;
    private Camera mainCamera;
    private Animator animator;
    [SerializeField] private GameObject InventoryPanels;
    
    private float mineCastRangeBuff = 0f;
    private float mineMultiplyBuff = 0f;
    private float attackRangeBuff = 0f;
    private float attackBuff = 0f;

    private bool isInventoryOpen;
    
    private void OnEnable()
    {
        inventory.ContainerUpdated += UpdateBuffsData;
        inventory.WeaponSet += SetWeapon;
        inventory.WeaponRemoved += RemoveWeapon;
    }

    private void OnDisable()
    {
        inventory.ContainerUpdated -= UpdateBuffsData;
        inventory.WeaponSet -= SetWeapon;
        inventory.WeaponRemoved -= RemoveWeapon;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        inventory = GetComponent<Container>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        isInventoryOpen = InventoryPanels.activeSelf;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isInventoryOpen)
        {
            animator.SetTrigger("Hit");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            HideInventory();
        }
    }

    private void SetWeapon(GameObject itemWorldPrefab)
    {
        //GameObject prefab = inventory.FindObjectInDatabase(weapon).WorldPrefab;
        weapon = Instantiate(itemWorldPrefab, WeaponHandlerGameObject.transform);
    }

    private void RemoveWeapon()
    {
        Destroy(weapon);
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

    private void HideInventory()
    {
        InventoryPanels.SetActive(!InventoryPanels.activeSelf);
        isInventoryOpen = InventoryPanels.activeSelf;
        InventoryOpen?.Invoke();
    }

    public void PlayerHit()
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
    
}