using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarTimer : TimerController
{
    [SerializeField]
    Image m_fillBar = null;
    
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (m_fillBar != null)
        {
            float percentage = currentTime / startingTime;
            m_fillBar.fillAmount = percentage;
        }
    }
}
