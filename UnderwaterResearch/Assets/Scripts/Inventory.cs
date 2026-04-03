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
    }

    private void Use(int i)
    {
        slots[i].item.Use();
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

}
