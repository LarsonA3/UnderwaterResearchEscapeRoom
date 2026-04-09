using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles ceiling lights and player death
public class LightControlRoom1 : MonoBehaviour
{
    public GameObject player; // for funny spin
    private bool isDone = false;
    Light[] lights;
    private float speed = 1.0f;
    private bool isdeath = false;

    private InputSystem_Actions input;
    public GameObject explosionref; // assign in inspector
    public GameObject reactorref; // assign in inspector

    private void Awake()
    {
        input = new InputSystem_Actions();
        input.Enable();
    }
    void Start()
    {
        isDone = false;
        lights = GetComponentsInChildren<Light>();
    }
    


    public float audioInterval = 2.0f; // seconds between siren
    private float timer;
    void Update()
    {
        if (!isDone)
        {
            float temp= Mathf.PingPong(Time.time * speed, 1);
            float curr = Mathf.Lerp(0.1f, 1.0f, temp);
            foreach (Light l in lights)
            {
                l.intensity = curr;
            }

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                this.GetComponent<AudioSource>().Play(); // Play the siren sound

                timer = audioInterval; // Reset the timer
            }
        }

        //CHEAT CODE TO DEATH
        if (input.UI.CheatCodeDeath.WasPressedThisFrame()) 
        {
            print("CHEAT CODE ACTIVATED - PLAYER DEATH");
            PlayerDeath();
        }
    }

    public void Done()
    {
        //turn lights back to normal
        isDone = true;
        this.transform.parent.parent.GetComponent<AudioSource>().Play();
        StartCoroutine(doneHelper());
    }

    private IEnumerator doneHelper() {
        foreach (Light light in lights)
        {
            light.intensity = 1.0f;
            light.color = new Color(0.855f, 0.992f, 0.698f);
            //set object to change from red to white
            var visualObjren = light.gameObject.transform.parent.GetComponent<Renderer>();
            visualObjren.material.SetColor("_BaseColor", Color.white);
        }
        for (int i = 0; i<10; i++)
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
            yield return new WaitForSeconds(0.25f);
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }


    }

    public void PlayerDeath()
    {
        if (isdeath) return;
        isDone = false;
        isdeath = true;

        StartCoroutine(DeathSeq());
    }

    private IEnumerator DeathSeq()
    {
        Debug.Log("PLAYER IS GOING TO DIE - SEQUENCE STARTED");

        // 1. Turn lights red (Immediate)
        foreach (Light light in lights)
        {
            if (light == null) continue;
            light.color = Color.red;
            var renderer = light.transform.parent.GetComponent<Renderer>();
            if (renderer != null) renderer.material.SetColor("_BaseColor", Color.red);
        }
        
        //play sound in reactor
        reactorref.GetComponent<AudioSource>().Play();

        // wait a few seconds
        yield return new WaitForSeconds(10.0f);

        //play explosion SOUND
        explosionref.GetComponent<AudioSource>().Play();

        print("shaking");
        //Screen Shake
        float deathTimer = 0.8f;
        Vector3 randomAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while (deathTimer > 0)
        {
            if (player != null)
            {
                randomAxis += Random.insideUnitSphere * 0.5f;
                float currentSpeed = 600f + (Random.Range(-100f, 100f));
                player.transform.Rotate(randomAxis * currentSpeed * Time.deltaTime);

                if (Camera.main != null)
                {
                    Camera.main.fieldOfView = 60f + Random.Range(-5f, 5f);
                }
            }

            deathTimer -= Time.deltaTime;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LoseScreen");
    }

    public bool isDead()
    {
        if (isdeath) {return true;}
        else { return false; }
    }
}
