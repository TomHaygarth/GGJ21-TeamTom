using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisablePlugin : MonoBehaviour
{

    public void disablePlugin()
    {
        LowteckTTS.TTS_Interface.DisablePlugin();
    }

    public void enablePlugin()
    {
        LowteckTTS.TTS_Interface.EnablePlugin();
    }
}
