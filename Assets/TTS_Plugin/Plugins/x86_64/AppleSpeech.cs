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
    public static extern bool IsSpeaking();
}
