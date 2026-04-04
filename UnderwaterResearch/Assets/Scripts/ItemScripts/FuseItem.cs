using UnityEngine;

public class FuseItem : InventoryItem {
    public float reachDistance = 3f;
    private Renderer objRenderer;

    private string fuseColor;
    
    private void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (this.transform.Find("RED"))
        {
            fuseColor = "RED";
            objRenderer.material.color = Color.red;
        }
        else if (this.transform.Find("BLUE"))
        {
            fuseColor = "BLUE";
            objRenderer.material.color = Color.blue;
        }
        else if (this.transform.Find("YELLOW"))
        {
            fuseColor = "YELLOW";
            objRenderer.material.color = Color.yellow;
        } 
        else if (this.transform.Find("GREEN"))
        {
            fuseColor = "GREEN";
            objRenderer.material.color = Color.green;
        }
        else
        {
            print("Error: Fuse item does not have a color");
        }
    }
    public override void Use()
    {
        print("using fuse");
        //raycast from main camera to see if looking at the printer
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (hit.collider.GetComponent<FuseBox>())
            {
                FuseBox fusebox = hit.collider.GetComponent<FuseBox>();
                //if used on printer, add filament to printer (send message to call addFilament()) and remove from inventory
                fusebox.addFuse(fuseColor);
                print("Filament added to printer by filament item");
            }
        }
    }
}
