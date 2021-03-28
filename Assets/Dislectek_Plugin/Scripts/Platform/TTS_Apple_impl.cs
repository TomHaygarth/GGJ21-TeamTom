#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
using UnityEngine;
using System.Collections.Generic;

namespace Dislectek
{
    public class TTS_Apple_impl : ITTSImpl
    {
        private List<ttsVoiceSelect> voiceDescriptions = new List<ttsVoiceSelect>();
        private const float default_words_per_minute = 180.0f;

        public void Init()
        {
            AppleSpeech.CreateSpeech();

            string[] availableVoices = AppleSpeech.GetVoices();
            for (int i = 0; i < availableVoices.Length; ++i)
            {
                voiceDescriptions.Add(new ttsVoiceSelect(availableVoices[i], i));
            }

            AppleSpeech.SetWordsPerMin(default_words_per_minute);
        }

        public void CleanUp()
        {
            AppleSpeech.DestroySpeech();
        }

        public int GetCurrentVoiceID()
        {
            return AppleSpeech.GetCurrentVoiceIndex();
        }

        /**
         * getInstalledVoices will return a list of installed system voices, which can be selected for voice output
         * */
        public List<ttsVoiceSelect> GetInstalledVoices()
        {
            return voiceDescriptions;
        }

        /**playTTS will trigger the TTS system to speak the provided text using the set volume, rate and voice 
        */
        public void PlayTTS(string text)
        {
            AppleSpeech.Speak(text);
        }

        /**setTTSVoice sets the TTS Voice to the provided voice id
         */
        public void SetTTSVoice(int id)
        {
            AppleSpeech.SetVoiceIndex(id);
        }

        /**setTTSRate sets the rate of the TTS 
         * parameter newTTSRate should be from -5 for very slow to 5 to very fast, with 0 being normal rate
       */
        public void SetTTSRate(int newTTSRate)
        {
            const float rate_multiplier = 15.0f;
            AppleSpeech.SetWordsPerMin(default_words_per_minute + (rate_multiplier * newTTSRate));
        }

        /**setTTSVolume sets the rate of the TTS 
        * parameter newTTSVolume should be from 1 for very quiet to 100 for normal
        * a volume of 0 will mute the speech
        */
        public void SetTTSVolume(int newTTSVolume)
        {
            float vol = newTTSVolume / 100.0f;
            AppleSpeech.SetVolume(vol);
        }

        /**
         * pauseSpeech will pause the current speech being played
         */
        public void PauseSpeech()
        {
            Debug.Log("Pausing..." + IsSpeaking());
            if (IsSpeaking())
                AppleSpeech.Pause();
        }
        /**
         * resumeSpeech will resume any speech currently paused
         */
        public void ResumeSpeech()
        {
            Debug.Log("Resuming..." + IsSpeaking());
            if (!IsSpeaking())
                AppleSpeech.Resume();
        }

        /**
         * skipToNextSentence will skip to the next sentence in the text currently being spoken
         */
        public void SkipToNextSentence()
        {
            Debug.Log("Skipping..." + IsSpeaking());
            AppleSpeech.SkipToNextSentence();
        }

        /**
         * skipToPreviousSentence will skip to the previous sentence in the text currently being spoken
         */
        public void SkipToPreviousSentence()
        {
            Debug.Log("Skipping..." + IsSpeaking());
            AppleSpeech.SkipToPrevSentence();
        }

        /**
         * skipToEnd will skip to the end of the text spoken
         */
        public void SkipToEnd()
        {
            AppleSpeech.Stop();
        }

        public bool IsSpeaking()
        {
            return AppleSpeech.IsSpeaking();
        }
    }
}
#endif