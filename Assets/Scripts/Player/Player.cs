using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public event Action InventoryOpen;
    
    public GameObject WeaponHandlerGameObject;
    public Image HealthBar;
    
    public float MineCastRange;
    public float MineMultiply;
    public float AttackRange;
    public float Attack;
    
    public float Health { get; set; }
    private float maxHealth;
    
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
        HealthBar.color = Color.green;
        maxHealth = 100;
        Health = maxHealth;
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
            //TODO тут можно лучше
            if (hit.collider.CompareTag("Resources"))
            {
                WorldItem worldItem = hit.collider.gameObject.GetComponent<WorldItem>();
                Item item = new Item(worldItem.ItemId, worldItem.Name, worldItem.GiveResources());
                inventory.AddItemInInventory(item);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                enemy.GetDamage(Attack);
            }
        }
    }

    public void HealthBarFill()
    {
        HealthBar.fillAmount = Health / maxHealth;
        Color healthColor = Color.Lerp(Color.red, Color.green, (Health / maxHealth));
        HealthBar.color = healthColor;
    }
}