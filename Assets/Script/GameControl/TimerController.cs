using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    float currentTime = 0f;
    [SerializeField]
    float startingTime = 20f;

    [SerializeField] Text countdownText;

    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        // just to prevent negative values
        if (currentTime <= 0)
        {
            currentTime = 0;
        }

        // making the color red when under 5 sec
        if (currentTime <= 5)
        {
            countdownText.color = Color.red;
        }


        // Scene change or event pop when time reaches zero
        /*
        if (currentTime = 0)
        {
            SceneManager.NextScene
        }
        */
    }
}
