using UnityEngine;

public class FuseBox : MonoBehaviour
{
    private bool opened = false; //CANNOT INTERACT W FUSES BEFOR THIS IS TRUE
    private GameObject boxDoor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxDoor = transform.Find("Door").gameObject;
    }

    public void Open()
    {
        if (!opened)
        {
            Destroy(boxDoor);
            opened = true;

            //set up fuse slots
        }
    }
}
