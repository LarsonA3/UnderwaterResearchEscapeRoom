using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //other scripts can access this
    public static int Held; // 0 - None, 1 - Multitool, 2- Lure Module

    public static bool MultitoolUnlocked = false; //use these to unlock items at the correct gameplay times.
    public static bool LureUnlocked = false; 


    private InputSystem_Actions input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Held = 0; // plr starts holding nothing

        input = new InputSystem_Actions();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Player.InventoryEquip1.WasPressedThisFrame())
        {
            // no item is always unlocked..duh
            Held = 0;
            print("Player equipped slot 1");
        } else if (input.Player.InventoryEquip2.WasPressedThisFrame())
        {
            if (MultitoolUnlocked)
            {
                Held = 1;
                print("Player equipped slot 2");
            }
        } else if (input.Player.InventoryEquip3.WasPressedThisFrame())
        {
            if (LureUnlocked)
            {
                Held = 2;
                print("Player equipped slot 3");
            }
        }

    }


}
