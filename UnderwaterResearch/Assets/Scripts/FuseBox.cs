using UnityEngine;

public class FuseBox : MonoBehaviour
{
    private bool opened = false; //CANNOT INTERACT W FUSES BEFOR THIS IS TRUE
    private GameObject boxDoor;

    public GameObject lockedDoor; //SET IN inspector, door to be unlocked

    private string firstslot = "";
    private string secondslot = "";
    private string thirdslot = "";

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

    public void addFuse(string color)
    {
        if (opened == true)
        {
            if (firstslot == "")
            {
                firstslot = color;
                print(color + " fuse added to first slot");
                //add visual
            }
            else if (secondslot == "")
            {
                secondslot = color;
                print(color + " fuse added to second slot");
                //add visual
            }
            else if (thirdslot == "")
            {
                thirdslot = color;
                print(color + " fuse added to third slot");
                checkForCombination();
                //add visual
            }
            else
            {
                print("no more slots availiable");
            }
            checkForCombination();
        }
        else
        {
             print("fuse box is not opened, cannot add fuses");
        }
    }

    public void removeFuse()
    {
        if (opened == true)
        {
            //remove fuse from slot and add back to inventory
            if (thirdslot == "")
            {
                print(thirdslot + " fuse removed from third slot");
                thirdslot = "";
                //remove visual

                //add item to inventory

            }
            else if (secondslot == "")
            {
                print(secondslot + " fuse removed from third slot");
                secondslot = "";
                //remove visual

                //add item to inventory
            }
            else if (firstslot == "")
            {
                print(firstslot + " fuse removed from second slot");
                firstslot = "";
                //remove visual

                //add item to inventory
            }
            else
            {
                print("There are no fuses for player to remove");
            }
        }
        else
        {
            print("fuse box is not opened, cannot remove fuses");
        }
    }


    private void checkForCombination()
    {
        if (firstslot == "BLUE" && secondslot == "RED" && thirdslot == "YELLOW") 
        {
            print("correct fuse combination!");
            Destroy(lockedDoor); //removes locked empty game object so door opens
            print("Unlocked main door");
        }
    }
}
