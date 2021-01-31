using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTimer : TimerController
{
    [SerializeField]
    TMPro.TMP_Text m_text = null;

    [SerializeField]
    string m_prefix = "Time: ";

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (m_text != null)
        {
            var ts = TimeSpan.FromSeconds(currentTime);
            m_text.text = string.Format("{0}{1:0}:{2:00}", m_prefix, ts.Minutes, ts.Seconds);
        }
    }
}