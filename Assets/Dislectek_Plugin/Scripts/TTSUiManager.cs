using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Dislectek
{
    public class TTSUiManager : MonoBehaviour
    {
        GameObject optionsObj;
        public Image BtnRead;
        public GameObject waveOn, waveOff;
        TTS parentTTS;
        public static bool optionsActive = false;
        Dropdown dropdown_;
        //bool talkOnExitToggle;
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

            //grab TTS_interface
        }

        public void toggleOptions()
        {

            //if (extractTextOnClick.canRead) toggleCanRead();

            optionsActive = !optionsActive;
            if (optionsActive) ActivateOptions();
            else DeactivateOptions();
        }
/*
        //talk on exit
        public void toggleTalkOnExitFtn(bool newValue)
        {
            //print("toggleTalkOnExit");
            //TalkOnExit(newValue);
        }

        public void togglePauseWhenActiveFtn(bool newValue)
        {
            /print("togglePause");
            //TalkOnExit(newValue);
        }
*/

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