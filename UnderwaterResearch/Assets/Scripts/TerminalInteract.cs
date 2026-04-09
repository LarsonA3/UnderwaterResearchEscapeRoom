using UnityEngine;

public class TerminalInteract : MonoBehaviour
{
  [SerializeField] private GameObject terminalUI;

  void Start()
  {
    if (terminalUI != null)
      terminalUI.SetActive(false);
  }

  void Grab()
  {
    if (terminalUI == null) return;
    terminalUI.SetActive(true);
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
  }
}