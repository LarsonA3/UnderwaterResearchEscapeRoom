using Unity.VisualScripting;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    // This is a fsm for an unlocked sliding door
    // DO NOT RENAME "DOOR" OR "LOCKED" CHILDREN IN INSPECTOR! THIS WILL BREAK THE DOOR!

    private GameObject door;

    private Vector3 Openpos = new Vector3(0,0,-1.541f);
    private Vector3 Closedpos = new Vector3(0, 0, 0);

    public float speed = 2f;
    private bool locked = false;

    enum DoorState
    {
        Open,
        Closing,
        Closed,
        Opening,
    }
    DoorState state;


    private void Start()
    {
        state = DoorState.Closed;
        door = this.transform.Find("Door").gameObject; 
            
        //see if door is locked
        if (this.transform.Find("Locked")) { locked = true; } 
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.transform.Find("Locked"))
        {
            locked = false;
        }
        switch (state)
        {
            case DoorState.Open:
                break;

            case DoorState.Closed:
                break;

            case DoorState.Opening:
                door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, Openpos, speed * Time.deltaTime);

                // Use distance check instead of == 
                if (Vector3.Distance(door.transform.localPosition, Openpos) < 0.01f)
                {
                    door.transform.localPosition = Openpos; // snap to exact position
                    TransitionTo(DoorState.Open);
                }
                break;

            case DoorState.Closing:
                door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, Closedpos, speed * Time.deltaTime);

                if (Vector3.Distance(door.transform.localPosition, Closedpos) < 0.01f)
                {
                    door.transform.localPosition = Closedpos; // snap to exact position
                    TransitionTo(DoorState.Closed);
                }
                break;
        }
    }

    void TransitionTo(DoorState newstate)
    {
        switch (state)
        {
            case DoorState.Open:
                if (newstate == DoorState.Closing) { state = newstate; print("Door Closing"); }
                break;
            case DoorState.Closing:
                if (newstate == DoorState.Closed) { state = newstate; }
                break;
            case DoorState.Closed:
                if (newstate == DoorState.Opening && locked == false) { state = newstate; print("Door opening"); }
                break;
            case DoorState.Opening:
                if (newstate == DoorState.Open) { state = newstate; }
                break;

        }
    }

    void Grab()
    {
        if (state == DoorState.Closed) { TransitionTo(DoorState.Opening); }
        else if (state == DoorState.Open) { TransitionTo(DoorState.Closing); }
    }


    void Unlock()
    {
        print("door unlocked");
        locked = false;
    }
}


    
