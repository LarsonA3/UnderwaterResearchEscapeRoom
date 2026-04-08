using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles ceiling lights and player death
public class LightControlRoom1 : MonoBehaviour
{

    private bool isDone = false;
    Light[] lights;
    private float speed = 1.0f;

    private InputSystem_Actions input;

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
        }

        //CHEAT CODE TO DEATH
        if (input.UI.CheatCodeDeath.WasPressedThisFrame()) 
        {
            PlayerDeath();
        }
    }

    public void Done()
    {
        //turn lights back to normal
        isDone = true;
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
        //turn lights red and make them flicker
        isDone = true;
        print("PLAYER IS GOING TO DIE - PLAYER DEATH CALLED");
        foreach (Light light in lights)
        {
            //turns lights red and color of object red
            light.color = Color.red;
            var visualObjren = light.gameObject.transform.parent.GetComponent<Renderer>();
            visualObjren.material.SetColor("_BaseColor", Color.red);


            //you lose screen


            //screen shake

            SceneManager.LoadScene("LoseScreen");

        }
    }
}
