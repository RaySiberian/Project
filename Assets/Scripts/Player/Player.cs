using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject rightHandItem;
    private GameObject leftHandItem;
    private float rayCastRange = 5f;
    private Container inventory;
    [SerializeField] private GameObject InventoryPanels;


    public Transform rightHandTransform;
    public Transform leftHandShieldTransform;
    public Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        inventory = GetComponent<Container>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerHit();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HideInventory();
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
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, rayCastRange))
        {
            if (hit.collider.CompareTag("Resources"))
            {
                WorldItem worldItem = hit.collider.gameObject.GetComponent<WorldItem>();
                Item item = new Item(worldItem.ItemId, worldItem.Name,  worldItem.GiveResources());
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