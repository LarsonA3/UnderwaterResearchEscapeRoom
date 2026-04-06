using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Printer : MonoBehaviour
{

    private GameObject filamentVisual; // the visual representation of the filament being added to the printer
    
    public GameObject MultiToolprefab; // set in inspector

    private GameObject spawn;

    private void Start()
    {
        this.GetComponent<BoxCollider>().enabled = true;
        filamentVisual = transform.Find("Filament").gameObject;
        filamentVisual.gameObject.SetActive(false);
        spawn = transform.Find("SpawnLocationMultitool")?.gameObject;

    }

    // used when filament item used on it. adds filament visually and then "prints" multitool 
    public void AddFilament()
    {
        print("addFilament Called successfully");
        filamentVisual.SetActive(true);

        StartCoroutine(PrintMultitool());

    }

    private IEnumerator PrintMultitool()
    {
        //wait 10 seconds
        print("Printer is now Printing...");
        yield return new WaitForSeconds(10f);
        //spawn prefab

        //remove hitbox
        this.GetComponent<BoxCollider>().enabled = false;
        Instantiate(MultiToolprefab, MultiToolprefab.transform.position, Quaternion.identity);
        print("Printer is done Printing!");
    }

}