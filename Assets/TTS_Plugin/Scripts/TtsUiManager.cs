using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LowteckTTS
{
    public class TtsUiManager : MonoBehaviour
    {
        GameObject optionsObj;
        public Image BtnRead;
        public GameObject waveOn, waveOff;
        TTS parentTTS;
        public static bool optionsActive = false;
        Dropdown dropdown_;
        List<TTS.ttsVoiceSelect> voice_list;
        public void Start()
        {
            optionsObj = transform.Find("Options").gameObject;
            parentTTS = GetComponentInParent<TTS>();
            //BtnRead = transform.Find("BtnRead").GetComponent<Image>();
            dropdown_ = GetComponentInChildren<Dropdown>(true);

            // Set dropdown options
            voice_list = parentTTS.getInstalledVoices();
            List<string> str_ls = new List<string>();
            foreach (var l in voice_list)
                str_ls.Add(l.description);
            //print(str_ls.Count);
            dropdown_.AddOptions(str_ls);
        }
        public void toggleOptions()
        {

            //if (extractTextOnClick.canRead) toggleCanRead();

            optionsActive = !optionsActive;
            if (optionsActive) ActivateOptions();
            else DeactivateOptions();
        }

        void ActivateOptions()
        {
            optionsActive = true;
            optionsObj.SetActive(true);
        }

        public void DeactivateOptions()
        {
            optionsActive = false;
            optionsObj.SetActive(false);
        }

        // discontinued
        public void toggleCanRead()
        {
            /*if (optionsActive) toggleOptions();

            extractTextOnClick.canRead = !extractTextOnClick.canRead;
            if (extractTextOnClick.canRead)        
                BtnRead.color = Color.red;
            else
                BtnRead.color = Color.white;
                */
        }

        public void changeVoiceType(System.Int32 I)
        {
            parentTTS.setTTSVoice(voice_list[I].id);
        }

        public void changeVolume(System.Single I)
        {
            parentTTS.setTTSVolume((int)I);
        }

        public void changeSpeed(System.Single I)
        {
            parentTTS.setTTSRate((int)I);
        }

        public void startReading()
        {
            waveOn.SetActive(true);
            waveOff.SetActive(false);
        }

        public void stopReading()
        {
            waveOn.SetActive(false);
            waveOff.SetActive(true);
        }
    }
}