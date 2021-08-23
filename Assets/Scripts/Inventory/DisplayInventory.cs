using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventoryToDisplay;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItems;
    public int ySpaceBetweenItems;
    public int numberOfColumn;

    private Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < inventoryToDisplay.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventoryToDisplay.Container[i]))
            {
                itemsDisplayed[inventoryToDisplay.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text =
                    inventoryToDisplay.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryToDisplay.Container[i].item.prefab, Vector3.zero, Quaternion.identity,
                    transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryToDisplay.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventoryToDisplay.Container[i],obj);
            }
        }
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventoryToDisplay.Container.Count; i++)
        {
            var obj = Instantiate(inventoryToDisplay.Container[i].item.prefab, Vector3.zero, Quaternion.identity,
                transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventoryToDisplay.Container[i].amount.ToString("n0");
            
            itemsDisplayed.Add(inventoryToDisplay.Container[i],obj);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems*(i % numberOfColumn)), yStart + (-ySpaceBetweenItems * (i/numberOfColumn)),0f);
    }
}
