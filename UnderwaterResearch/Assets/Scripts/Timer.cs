using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int TimeLimit = 440; //4 mins time limit
    private int currentTime;

    //private bool failed = false;

    public GameObject lightref; // set in inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = true;
        text.text = TimeLimit.ToString();
        currentTime = TimeLimit;
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private IEnumerator Countdown()
    {
        while (currentTime > 0)
        {

            yield return new WaitForSeconds(1);
            //update text
            currentTime --;
            text.text = currentTime.ToString();
        }
        
        //failed = true;
        //set time to 0
        currentTime = 0;
        text.text = "0";
        //fail and call light control
        var lig = lightref.GetComponent<LightControlRoom1>();
        lig.PlayerDeath();

    }
}
