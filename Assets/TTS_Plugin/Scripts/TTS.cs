using UnityEngine;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using SpeechLib;
#endif

using System.Collections.Generic;

namespace LowteckTTS
{
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    public class TTS : MonoBehaviour
    {
        public enum ttsVoices { Male, Female };

        private int ttsVolume = 100;
        private int ttsRate = 0;
        private int ttsVoice = 0;

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

        private List<ttsVoiceSelect> voiceDescriptions = new List<ttsVoiceSelect>();

        void Awake()
        {
            AppleSpeech.CreateSpeech();
            voiceDescriptions.Add(new ttsVoiceSelect("Default voice", 0));
        }

        private void OnDestroy()
        {
            AppleSpeech.DestroySpeech();
        }

        /**
         * getInstalledVoices will return a list of installed system voices, which can be selected for voice output
         * */
        public List<ttsVoiceSelect> getInstalledVoices()
        {

            return voiceDescriptions;
        }

        /**playTTS will trigger the TTS system to speak the provided text using the set volume, rate and voice 
        */
        public void playTTS(string text)
        {
            AppleSpeech.Speak(text);
        }


        /**setTTSVoice sets the TTS Voice to the provided voice id
         */
        public void setTTSVoice(int id)
        {
            // TODO
        }
        /**setTTSVoice sets the TTS Voice to the provided voice type
       */
        public void setTTSVoice(ttsVoices newTTSVoice)
        {
            // TODO
        }

        /**setTTSRate sets the rate of the TTS 
         * parameter newTTSRate should be from -5 for very slow to 5 to very fast, with 0 being normal rate
       */
        public void setTTSRate(int newTTSRate)
        {
            //TODO
        }

        /**setTTSVolume sets the rate of the TTS 
        * parameter newTTSVolume should be from 1 for very quiet to 100 for normal
        * a volume of 0 will mute the speech
        */
        public void setTTSVolume(int newTTSVolume)
        {
            // TODO
        }

        /**
         * pauseSpeech will pause the current speech being played
         */
        public void pauseSpeech()
        {
            Debug.Log("Pausing..." + isSpeaking());
            if (isSpeaking())
                AppleSpeech.Pause();
        }
        /**
         * resumeSpeech will resume any speech currently paused
         */
        public void resumeSpeech()
        {
            Debug.Log("Resuming..." + isSpeaking());
            if (!isSpeaking())
                AppleSpeech.Resume();
        }

        /**
         * skipToNextSentence will skip to the next sentence in the text currently being spoken
         */
        public void skipToNextSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
        }

        /**
         * skipToPreviousSentence will skip to the previous sentence in the text currently being spoken
         */
        public void skipToPreviousSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
        }

        /**
         * skipToEnd will skip to the end of the text spoken
         */
        public void skipToEnd()
        {
            //TODO
        }

        public bool isSpeaking()
        {
            return AppleSpeech.IsSpeaking();
        }
    }
#else
    public class TTS : MonoBehaviour
    {
        public enum ttsVoices { Male, Female };

        private int ttsVolume = 100;
        private int ttsRate = 0;
        private int ttsVoice = 0;

        private SpVoice spVoice;

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

        private List<ttsVoiceSelect> voiceDescriptions = new List<ttsVoiceSelect>();

        void Awake()
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

        /**
         * getInstalledVoices will return a list of installed system voices, which can be selected for voice output
         * */
        public List<ttsVoiceSelect> getInstalledVoices()
        {

            return voiceDescriptions;
        }

        /**playTTS will trigger the TTS system to speak the provided text using the set volume, rate and voice 
        */
        public void playTTS(string text)
        {
            if (spVoice != null)
                spVoice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            else
                Debug.LogError("ERROR: SpVoice is null");

        }


        /**setTTSVoice sets the TTS Voice to the provided voice id
         */
        public void setTTSVoice(int id)
        {

            ISpeechObjectTokens vv = spVoice.GetVoices();

            if (id < 0 || id >= vv.Count)
            {
                Debug.LogError("ERROR: No installed voice with id " + id);
                return;

            }
            spVoice.Voice = vv.Item(id);

        }
        /**setTTSVoice sets the TTS Voice to the provided voice type
       */
        public void setTTSVoice(ttsVoices newTTSVoice)
        {

            if (newTTSVoice == ttsVoices.Male)
                ttsVoice = 0;
            else if (newTTSVoice == ttsVoices.Female)
                ttsVoice = 1;

            ISpeechObjectTokens vv = spVoice.GetVoices();
            spVoice.Voice = vv.Item(ttsVoice);

        }

        /**setTTSRate sets the rate of the TTS 
         * parameter newTTSRate should be from -5 for very slow to 5 to very fast, with 0 being normal rate
       */
        public void setTTSRate(int newTTSRate)
        {
            if (newTTSRate < -5)
                newTTSRate = -5;
            else if (newTTSRate > 5)
                newTTSRate = 5;

            ttsRate = newTTSRate;

            spVoice.Rate = ttsRate; // Rate - 5 to 5 
        }

        /**setTTSVolume sets the rate of the TTS 
        * parameter newTTSVolume should be from 1 for very quiet to 100 for normal
        * a volume of 0 will mute the speech
        */
        public void setTTSVolume(int newTTSVolume)
        {
            if (newTTSVolume < 0)
                newTTSVolume = 0;
            else if (newTTSVolume > 100)
                newTTSVolume = 100;

            ttsVolume = newTTSVolume;

            spVoice.Volume = ttsVolume; // Volume - 0 to 100
        }

        /**
         * pauseSpeech will pause the current speech being played
         */
        public void pauseSpeech()
        {
            Debug.Log("Pausing..." + isSpeaking());
            if (isSpeaking())
                spVoice.Pause();
        }
        /**
         * resumeSpeech will resume any speech currently paused
         */
        public void resumeSpeech()
        {
            Debug.Log("Resuming..." + isSpeaking());
            if (!isSpeaking())
                spVoice.Resume();
        }

        /**
         * skipToNextSentence will skip to the next sentence in the text currently being spoken
         */
        public void skipToNextSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
            if (isSpeaking())
                spVoice.Skip("Sentence", 1);
        }

        /**
         * skipToPreviousSentence will skip to the previous sentence in the text currently being spoken
         */
        public void skipToPreviousSentence()
        {
            Debug.Log("Skipping..." + isSpeaking());
            if (isSpeaking())
                spVoice.Skip("Sentence", -1);
        }

        /**
         * skipToEnd will skip to the end of the text spoken
         */
        public void skipToEnd()
        {
            if (isSpeaking())
                spVoice.Skip("Sentence", 999); // Brute force way to stop, given lack of Stop funciton in the lib
        }

        public bool isSpeaking()
        {
            return (spVoice.Status.RunningState == SpeechRunState.SRSEIsSpeaking);
        }
    }
#endif
}
