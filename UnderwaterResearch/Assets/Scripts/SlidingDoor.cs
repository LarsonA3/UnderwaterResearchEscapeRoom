using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    // This is a fsm for an unlocked sliding door

    enum DoorState
    {
        Open,
        Closing,
        Closed,
        Opening,
    }
    DoorState state;


    private void Awake()
    {
        state = DoorState.Closed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DoorState.Open:
                break;
            case DoorState.Closing:
                break;
            case DoorState.Closed:
                break;
            case DoorState.Opening:
                break;
            default:
                state = DoorState.Closed;
                break;

        }
    }

    void TransitionTo(DoorState newstate)
    {
        switch(state)
        {
            case DoorState.Open:
                if (newstate == DoorState.Closing) state = newstate;
                break;
            case DoorState.Closing:
                if (newstate == DoorState.Closed) state = newstate;
                break;
            case DoorState.Closed:
                if (newstate == DoorState.Opening) state = newstate;
                break;
            case DoorState.Opening:
                if (newstate == DoorState.Open) state = newstate;
                break;

        }
    }
}
