using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int Amount;

    public int AmountMultiplayerByDurability;
    public int Durability = 5;
    private int tes;
    private void Start()
    {
        Amount = Durability * AmountMultiplayerByDurability;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(tes = GiveResources());
        }
    }

    public int GiveResources()
    {
        int returnNumber = Amount / Durability;
        Amount -= returnNumber;
        Durability --;
        if (Durability == 0)
        {
            Destroy(gameObject);
        }
        return returnNumber;
    }
}
