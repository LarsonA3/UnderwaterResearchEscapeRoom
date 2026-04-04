using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class InventorySlot
{
    // set in inspector
    public Image img;

    [HideInInspector] public InventoryItem item;
}


public class Inventory : MonoBehaviour
{
    // set in inspector
    public List<InventorySlot> slots;

    // other fields/properties
    public static Inventory Instance { get; private set; }
    private InputSystem_Actions input;

    private void Awake()
    {
        Instance = this;
        input = new();
        input.Enable();
        input.UI.Enable();
    }

    private void Update()
    {
        if (input.UI.UseInventoryItem1.WasPressedThisFrame())
        {
            Use(0);
        }
        if (input.UI.UseInventoryItem2.WasPressedThisFrame())
        {
            Use(1);
        }
        if (input.UI.UseInventoryItem3.WasPressedThisFrame())
        {
            Use(2);
        }
        if (input.UI.UseInventoryItem4.WasPressedThisFrame())
        {
            Use(3);
        }
        if (input.UI.UseInventoryItem5.WasPressedThisFrame())
        {
            Use(4);
        }
    }

    private void Use(int i)
    {
        if (slots[i].item == null)
        {
            //print("HEY! No item in slot " + (i+1));
        } else
        {
            slots[i].item.Use();
            print("used item in slot " + (i + 1));
        }
    }

    public bool Add(InventoryItem item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].img.gameObject.SetActive(true);
                slots[i].img.sprite = item.sprHud;
                return true;
            }
        }
        return false;
    }

    public void Remove(int i)
    {
        if (i < 0 || i >= slots.Count) return;
        if (slots[i].item != null)
        {
            print("Removed item from slot " + (i + 1));

            //clear data reference
            slots[i].item = null;

            //reset the UI
            slots[i].img.sprite = null;
            slots[i].img.gameObject.SetActive(false);
        }
    }

}
