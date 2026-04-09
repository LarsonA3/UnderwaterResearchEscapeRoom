using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject Obj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the win screen scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
            Animation End = Obj.AddComponent<Animation>();
            End.Play("Float");
        }

    }
}
