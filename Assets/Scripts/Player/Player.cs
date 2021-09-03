using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject rightHandItem ;
    private GameObject leftHandItem ;
    private float rayCastRange = 5f;
    
    public Transform rightHandTransform;
    public Transform leftHandShieldTransform;
    public Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shot();
        }
    }
    
    private void Shot()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position,mainCamera.transform.forward,out hit,rayCastRange))
        {
            if (hit.collider.CompareTag("Resources"))
            {
                Debug.Log("Ресурсы");
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