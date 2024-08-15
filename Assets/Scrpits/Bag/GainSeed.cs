using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainSeed : MonoBehaviour
{
    public List<CropData> thisCrop;
    public Inventory playerInventory;
    public int cropIndex =3;
    public void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisCrop[cropIndex]))
        {
            playerInventory.itemList.Add(thisCrop[cropIndex]);
        }
        InventoryManager.RefreshItem();
        cropIndex++;
    }
}
