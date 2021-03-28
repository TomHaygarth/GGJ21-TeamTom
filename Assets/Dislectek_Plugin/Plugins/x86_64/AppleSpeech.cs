using System;
using System.Runtime.InteropServices;

public class AppleSpeech
{
    [DllImport("__Internal")]
    public static extern void CreateSpeech();

    [DllImport("__Internal")]
    public static extern void DestroySpeech();

    [DllImport("__Internal")]
    public static extern void Speak([MarshalAs(UnmanagedType.LPUTF8Str)]string text);

    [DllImport("__Internal")]
    public static extern void Resume();

    [DllImport("__Internal")]
    public static extern void Pause();

    [DllImport("__Internal")]
    public static extern void Stop();

    [DllImport("__Internal")]
    public static extern bool IsSpeaking();

    public static string[] GetVoices()
    {
        int voice_count = AvailableVoiceCount();
        string[] voice_names = new string[voice_count];

        for(int i = 0; i < voice_count; ++i)
        {
            IntPtr str_ptr = GetVoiceNameAtIndex(i);
            voice_names[i] = Marshal.PtrToStringAuto(str_ptr);
        }
        return voice_names;
    }


    [DllImport("__Internal")]
    public static extern bool SetVoiceIndex(int idx);

    [DllImport("__Internal")]
    public static extern void SetVolume(float volume);

    [DllImport("__Internal")]
    public static extern void SetWordsPerMin(float wpm);

    [DllImport("__Internal")]
    public static extern void SkipToNextSentence();

    [DllImport("__Internal")]
    public static extern void SkipToPrevSentence();

    [DllImport("__Internal")]
    public static extern int GetCurrentVoiceIndex();

    [DllImport("__Internal")]
    private static extern int AvailableVoiceCount();

    [DllImport("__Internal")]
    private static extern IntPtr GetVoiceNameAtIndex(int idx);
}
