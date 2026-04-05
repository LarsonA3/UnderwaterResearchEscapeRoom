using UnityEngine;

public class QuitButton : MonoBehaviour
{


    public void OnClick()
    {
        print("Quitting game...");
        Application.Quit();
    }
}
