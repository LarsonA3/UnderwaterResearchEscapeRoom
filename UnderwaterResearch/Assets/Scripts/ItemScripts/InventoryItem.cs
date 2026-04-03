using UnityEngine;

public class InventoryItem : MonoBehaviour {
  public Sprite sprHud;

  public void Grab() {
    print("grabbed item");
    gameObject.SetActive(false);
    Inventory.Instance.Add(this);
  }

  public virtual void Use() {
    print("you shouldn't be calling this version of use");
  }
}
