using System;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public Recorder recorder;
    public SpeechToText speechToText;

    public ChatManager chatManager;

    private void Start()
    {
        recorder.onStopRecord = (audio) =>
        {
            speechToText.Convert(audio);
        };

        speechToText.onDetectSpeech = (text) =>
        {
            chatManager.NpcController.StartAnswerUser(text);
        };
    }
    
    public void StartRecorder()
    {
        recorder.StartRecording();
    }

    public void StopRecorder()
    {
        recorder.StopRecording();
    }
}
