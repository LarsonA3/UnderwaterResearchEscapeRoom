using UnityEngine;


//instant beat room 1
public class CheatCodes : MonoBehaviour
{
    private InputSystem_Actions input;

    public GameObject lock1;
    public GameObject lock2;
    void Start()
    {
        input = new InputSystem_Actions();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.UI.CheatCodeUnlock.WasPressedThisFrame())
        {
            print("CHEAT CODE ACTIVATED - UNLOCK ALL DOORS");
            Destroy(lock1);
            Destroy(lock2);
        }
    }
}
