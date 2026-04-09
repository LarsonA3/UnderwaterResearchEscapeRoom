using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the win screen scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
        }

    }
}
