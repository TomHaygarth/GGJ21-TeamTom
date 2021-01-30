using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTimer : TimerController
{
    [SerializeField]
    Text m_text = null;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (m_text != null)
        {
            m_text.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\\:ss");
        }
    }
}