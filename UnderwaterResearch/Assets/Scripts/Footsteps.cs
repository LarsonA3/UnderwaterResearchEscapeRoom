using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public float footstepInterval = 0.5f; // time btwn steps
    private float timer;
    private AudioSource audioSource;

    private InputSystem_Actions input;

    void Awake()
    {
        input = new InputSystem_Actions();
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable() { input.Enable(); }
    void OnDisable() { input.Disable(); }

    void Update()
    {
        bool isMoving = input.Player.Move.ReadValue<Vector2>().magnitude > 0.1f;

        if (isMoving)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (audioSource != null && !audioSource.isPlaying)
                {
                    // Random pitch
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.Play();
                }
                timer = footstepInterval;
            }
        }
        else
        {
            // Reset timerr
            timer = 0;
        }
    }
}