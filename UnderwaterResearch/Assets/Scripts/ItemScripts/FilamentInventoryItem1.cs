using UnityEngine;

public class FilamentInventoryItem : InventoryItem {
    public float reachDistance = 3f;

    public override void Use()
    {
        print("using filament...");
        //raycast from main camera to see if looking at the printer
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (hit.collider.GetComponent<Printer>())
            {
                Printer printer = hit.collider.GetComponent<Printer>();
                //if used on printer, add filament to printer (send message to call addFilament()) and remove from inventory
                printer.AddFilament();
                print("Filament added to printer by filament item");
                //remove from inventory
                Inventory.Instance.Remove(Inventory.Instance.slots.IndexOf(Inventory.Instance.slots.Find(slot => slot.item == this)));
            }
        }
    }
}
