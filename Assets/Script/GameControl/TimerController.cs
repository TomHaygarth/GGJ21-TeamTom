using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public delegate void TimerFinished();
    public event TimerFinished OnTimerFinished = delegate {};

    [SerializeField]
    protected float currentTime = 0f;
    [SerializeField]
    protected float startingTime = 20f;

    [SerializeField]
    bool m_isCounting = false;

    protected void Start()
    {
        currentTime = startingTime;
    }

    public void StartTimer()
    {
        currentTime = startingTime;
        m_isCounting = true;
    }

    public void Pause()
    {
        m_isCounting = false;
    }

    public void Resume()
    {
        m_isCounting = true;
    }

    protected void Update()
    {
        if (m_isCounting == false)
        {
            return;
        }

        currentTime -= 1 * Time.deltaTime;
        //countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
            OnTimerFinished();
            m_isCounting = false;
        }

    }
}
