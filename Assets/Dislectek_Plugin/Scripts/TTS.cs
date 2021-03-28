using UnityEngine;
using System.Collections.Generic;

namespace Dislectek
{
    public class TTS : MonoBehaviour
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        ITTSImpl m_tts = new TTS_Windows_impl();
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        ITTSImpl m_tts = new TTS_Apple_impl();
#endif

        void Awake()
        {
            m_tts.Init();
        }

        private void OnDestroy()
        {
            m_tts.CleanUp();
        }

        public int GetCurrentVoiceID()
        {
            return m_tts.GetCurrentVoiceID();
        }

        /**
         * getInstalledVoices will return a list of installed system voices, which can be selected for voice output
         * */
        public List<ttsVoiceSelect> getInstalledVoices()
        {
            return m_tts.GetInstalledVoices();
        }

        /**playTTS will trigger the TTS system to speak the provided text using the set volume, rate and voice 
        */
        public void playTTS(string text)
        {
            m_tts.PlayTTS(text);
        }


        /**setTTSVoice sets the TTS Voice to the provided voice id
         */
        public void setTTSVoice(int id)
        {
            m_tts.SetTTSVoice(id);
        }

        /**setTTSRate sets the rate of the TTS 
         * parameter newTTSRate should be from -5 for very slow to 5 to very fast, with 0 being normal rate
       */
        public void setTTSRate(int newTTSRate)
        {
            m_tts.SetTTSRate(newTTSRate);
        }

        /**setTTSVolume sets the rate of the TTS 
        * parameter newTTSVolume should be from 1 for very quiet to 100 for normal
        * a volume of 0 will mute the speech
        */
        public void setTTSVolume(int newTTSVolume)
        {
            m_tts.SetTTSVolume(newTTSVolume);
        }

        /**
         * pauseSpeech will pause the current speech being played
         */
        public void pauseSpeech()
        {
            Debug.Log("Pausing..." + isSpeaking());
            m_tts.PauseSpeech();
        }
        /**
         * resumeSpeech will resume any speech currently paused
         */
        public void resumeSpeech()
        {
            Debug.Log("Resuming..." + isSpeaking());
            m_tts.ResumeSpeech();
        }

        /**
         * skipToNextSentence will skip to the next sentence in the text currently being spoken
         */
        public void skipToNextSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
            m_tts.SkipToNextSentence();
        }

        /**
         * skipToPreviousSentence will skip to the previous sentence in the text currently being spoken
         */
        public void skipToPreviousSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
            m_tts.SkipToPreviousSentence();
        }

        /**
         * skipToEnd will skip to the end of the text spoken
         */
        public void skipToEnd()
        {
            m_tts.SkipToEnd();
        }

        public bool isSpeaking()
        {
            return m_tts.IsSpeaking();
        }
    }

    public class ttsVoiceSelect
    {
        public string description;
        public int id;

        public ttsVoiceSelect(string description, int id)
        {
            this.description = description;
            this.id = id;
        }
    }

    public interface ITTSImpl
    {
        /**
         * Init will Initialise the TTS plugin for the platform
         * */
        void Init();

        /**
         * CleanUp will perform any necessary cleanup of the TTS plugin
         * */
        void CleanUp();

        /**
        * GetCurrentVoiceID will return the ID of the currently set voice
        * */
        int GetCurrentVoiceID();

        /**
         * getInstalledVoices will return a list of installed system voices, which can be selected for voice output
         * */
        List<ttsVoiceSelect> GetInstalledVoices();

        /**playTTS will trigger the TTS system to speak the provided text using the set volume, rate and voice
        */
        void PlayTTS(string text);


        /**setTTSVoice sets the TTS Voice to the provided voice id
         */
        void SetTTSVoice(int id);

        /**setTTSRate sets the rate of the TTS
         * parameter newTTSRate should be from -5 for very slow to 5 to very fast, with 0 being normal rate
       */
        void SetTTSRate(int newTTSRate);

        /**setTTSVolume sets the rate of the TTS
        * parameter newTTSVolume should be from 1 for very quiet to 100 for normal
        * a volume of 0 will mute the speech
        */
        void SetTTSVolume(int newTTSVolume);

        /**
         * pauseSpeech will pause the current speech being played
         */
        void PauseSpeech();

        /**
         * resumeSpeech will resume any speech currently paused
         */
        void ResumeSpeech();

        /**
         * skipToNextSentence will skip to the next sentence in the text currently being spoken
         */
        void SkipToNextSentence();

        /**
         * skipToPreviousSentence will skip to the previous sentence in the text currently being spoken
         */
        void SkipToPreviousSentence();

        /**
         * skipToEnd will skip to the end of the text spoken
         */
        void SkipToEnd();

        bool IsSpeaking();
    }
}
