#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using SpeechLib;

using UnityEngine;
using System.Collections.Generic;

namespace Dislectek
{
    public class TTS_Windows_impl : ITTSImpl
    {
        private SpVoice spVoice;

        private int ttsVolume = 100;
        private int ttsRate = 0;
        private int ttsVoice = 0;

        private List<ttsVoiceSelect> voiceDescriptions = new List<ttsVoiceSelect>();

        public void Init()
        {
            //create the TTS SpVoice and set the defaults
            spVoice = new SpVoice();

            spVoice.Rate = ttsRate; // Rate - 5 to 5 
            spVoice.Volume = ttsVolume; // Volume - 0 to 100


            //get the system voices and store their description and a reference id
            ISpeechObjectTokens vv = spVoice.GetVoices();


            if (vv.Count == 0)
            {
                Debug.LogError("ERROR: Cannot detect any installed system voices");
            }
            else
            {
                // set default voice as the first in the list
                spVoice.Voice = vv.Item(ttsVoice);

                for (int v = 0; v < vv.Count; v++)
                {
                    voiceDescriptions.Add(new ttsVoiceSelect(vv.Item(v).GetDescription(), v));
                }

            }
        }

        public void CleanUp()
        {
            // NO-OP on windows
        }

        public int GetCurrentVoiceID()
        {
            return ttsVoice;
        }

        public List<ttsVoiceSelect> GetInstalledVoices()
        {
            return voiceDescriptions;
        }

        public void PlayTTS(string text)
        {
            if (spVoice != null)
                spVoice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            else
                Debug.LogError("ERROR: SpVoice is null");
        }

        public void SetTTSVoice(int id)
        {
            ISpeechObjectTokens vv = spVoice.GetVoices();
            ttsVoice = id;

            if (id < 0 || id >= vv.Count)
            {
                Debug.LogError("ERROR: No installed voice with id " + id);
                return;

            }
            spVoice.Voice = vv.Item(id);
        }

        public void SetTTSRate(int newTTSRate)
        {
            if (newTTSRate < -5)
                newTTSRate = -5;
            else if (newTTSRate > 5)
                newTTSRate = 5;

            ttsRate = newTTSRate;

            spVoice.Rate = ttsRate; // Rate - 5 to 5
        }

        public void SetTTSVolume(int newTTSVolume)
        {
            if (newTTSVolume < 0)
                newTTSVolume = 0;
            else if (newTTSVolume > 100)
                newTTSVolume = 100;

            ttsVolume = newTTSVolume;

            spVoice.Volume = ttsVolume; // Volume - 0 to 100
        }

        public void PauseSpeech()
        {
            if (IsSpeaking())
                spVoice.Pause();
        }

        public void ResumeSpeech()
        {
            if (!IsSpeaking())
                spVoice.Resume();
        }

        public void SkipToNextSentence()
        {
            if (IsSpeaking())
                spVoice.Skip("Sentence", 1);
        }

        public void SkipToPreviousSentence()
        {
            if (IsSpeaking())
                spVoice.Skip("Sentence", -1);
        }

        public void SkipToEnd()
        {
            if (IsSpeaking())
                spVoice.Skip("Sentence", 999); // Brute force way to stop, given lack of Stop funciton in the lib
        }

        public bool IsSpeaking()
        {
            return (spVoice.Status.RunningState == SpeechRunState.SRSEIsSpeaking);
        }
    }
}
#endif
