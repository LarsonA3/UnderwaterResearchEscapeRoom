using UnityEngine;

public class MultiTool : InventoryItem {
    public float reachDistance = 3f;

    public override void Use()
    {
        print("using multitool...");
        //raycast from main camera to see if looking at the printer
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (hit.collider.GetComponent<FuseBox>())
            {
                FuseBox box = hit.collider.GetComponent<FuseBox>();
                //if used on printer, add filament to printer (send message to call addFilament()) and remove from inventory
                box.Open();
                print("fuse box opened by multitool");
            }
        }
    }
}
