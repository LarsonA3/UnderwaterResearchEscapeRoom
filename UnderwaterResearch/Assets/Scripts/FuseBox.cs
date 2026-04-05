using UnityEngine;

public class FuseBox : MonoBehaviour
{
    private bool opened = false; //CANNOT INTERACT W FUSES BEFOR THIS IS TRUE
    private GameObject boxDoor;

    public GameObject lockedDoor; //SET IN inspector, door to be unlocked

    private GameObject slot1visual;
    private GameObject slot2visual;
    private GameObject slot3visual;

    private string firstslot = "";
    private string secondslot = "";
    private string thirdslot = "";

    public GameObject lightcontrol;


    private Renderer firstRen;
    private Renderer secondRen;
    private Renderer thirdRen;

    private bool done = false;

    //set in inspector
    public Sprite redFuseSprite;
    public Sprite blueFuseSprite;
    public Sprite yellowFuseSprite;
    public Sprite greenFuseSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxDoor = transform.Find("Door").gameObject;
        slot1visual = this.gameObject.transform.Find("Slot1")?.gameObject;
        slot2visual = this.gameObject.transform.Find("Slot2")?.gameObject;
        slot3visual = this.gameObject.transform.Find("Slot3")?.gameObject;
        firstRen = slot1visual.GetComponent<Renderer>();
        secondRen = slot2visual.GetComponent<Renderer>();
        thirdRen = slot3visual.GetComponent<Renderer>();
        done = false;
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
        if (opened && !done)
        {
            if (firstslot == "")
            {
                firstslot = color;
                print(color + " fuse added to first slot");
                //add visual
                slot1visual.SetActive(true);
                firstRen.material.color = color switch
                {
                    "RED" => Color.red,
                    "BLUE" => Color.blue,
                    "YELLOW" => Color.yellow,
                    "GREEN" => Color.green,
                    _ => firstRen.material.color
                };
            }
            else if (secondslot == "")
            {
                secondslot = color;
                print(color + " fuse added to second slot");
                //add visual
                slot2visual.SetActive(true);
                secondRen.material.color = color switch
                {
                    "RED" => Color.red,
                    "BLUE" => Color.blue,
                    "YELLOW" => Color.yellow,
                    "GREEN" => Color.green,
                    _ => secondRen.material.color
                };
            }
            else if (thirdslot == "")
            {
                thirdslot = color;
                print(color + " fuse added to third slot");
                //add visual
                slot3visual.SetActive(true);
                thirdRen.material.color = color switch
                {
                    "RED" => Color.red,
                    "BLUE" => Color.blue,
                    "YELLOW" => Color.yellow,
                    "GREEN" => Color.green,
                    _ => thirdRen.material.color
                };
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

    public void Grab()
    {
        //just remove fuse
        removeFuse();
    }

    public void removeFuse()
    {
        if (opened == true && !done)
        {
            //remove fuse from slot and add back to inventory
            if (thirdslot != "")
            {
                print(thirdslot + " fuse removed from third slot");
                //remove visual
                slot3visual.SetActive(false);
                //add item to inventory
                CreateAndAddFuse(thirdslot);
                thirdslot = "";
            }
            else if (secondslot != "")
            {
                print(secondslot + " fuse removed from second slot");
                //remove visual
                slot2visual.SetActive(false);

                //add item to inventory
                CreateAndAddFuse(secondslot);
                secondslot = "";
            }
            else if (firstslot != "")
            {
                print(firstslot + " fuse removed from first slot");
                //remove visual
                slot1visual.SetActive(false);
                //add item to inventory
                CreateAndAddFuse(firstslot);
                firstslot = "";
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

    //helper for removefuse
    private void CreateAndAddFuse(string color)
    {
        GameObject temp = new GameObject("FuseItem_Data");
        FuseItem tfuse = temp.AddComponent<FuseItem>();
        tfuse.setColor(color);
        tfuse.sprHud = color switch
        {
            "RED" => redFuseSprite,
            "BLUE" => blueFuseSprite,
            "YELLOW" => yellowFuseSprite,
            "GREEN" => greenFuseSprite,
            _ => null
        };

        temp.SetActive(false);
        Inventory.Instance.Add(tfuse);
    }


    private void checkForCombination()
    {
        if (firstslot == "BLUE" && secondslot == "RED" && thirdslot == "YELLOW") 
        {
            print("correct fuse combination!");
            Destroy(lockedDoor); //removes locked empty game object so door opens
            print("Unlocked main door");
            lightcontrol.GetComponent<LightControlRoom1>().Done(); //turn lights back to normal
            done = true;
        }
    }
}
