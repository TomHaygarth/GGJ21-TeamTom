using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableDislectekPlugin : MonoBehaviour
{
    public bool isPluginActive = true;

    public void disablePlugin()
    {
        Dislectek.TTS_Interface.DisablePlugin();
        
    }

    public void enablePlugin()
    {
        Dislectek.TTS_Interface.EnablePlugin();
    }

    public void togglePlugin()
    {
        

        isPluginActive = !isPluginActive;

        if (isPluginActive == true)
        {
            Dislectek.TTS_Interface.EnablePlugin();
        }
        if (isPluginActive == false)
        {
            Dislectek.TTS_Interface.DisablePlugin();
        }
    }
}
