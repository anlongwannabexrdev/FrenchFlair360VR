using System.Collections;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Extensions;
using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    [SerializeField] private int maxTimeToRecord = 30;
    
    private AudioClip recordedClip;
    private const int sampleRate = 16000;
    private SpeechToText speechToText;
    private TextToSpeech textToSpeech;
    private ChatBot chatBot;

    void Start()
    {
        speechToText = gameObject.GetOrAddComponent<SpeechToText>();
        textToSpeech = gameObject.GetOrAddComponent<TextToSpeech>();
        chatBot = gameObject.GetOrAddComponent<ChatBot>();

        speechToText.onDetectSpeech += (result) =>
        {
            chatBot.AskToBot(result);
        };

        chatBot.onRespond += (response) =>
        {
            textToSpeech.SpeechText(response);
        };
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }
        recordedClip = Microphone.Start(null, false, maxTimeToRecord, sampleRate);
        Debug.Log("Recording started...");
        StartCoroutine(_IEStopRecording());
    }

    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
            Microphone.End(null);
    }

    IEnumerator _IEStopRecording()
    {
        while (Microphone.IsRecording(null))
            yield return null;
        Debug.Log("Recording stopped.");

        speechToText.Convert(AudioConverter.ConvertAudioClipToByteArray(recordedClip));
    }

    bool isRecording = false;
    void OnGUI()
    {
        float buttonWidth = 200;
        float buttonHeight = 50;
        float spacing = 20;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        float startX = (screenWidth - buttonWidth) / 2;
        float startY = (screenHeight - (buttonHeight * 2 + spacing)) / 2;

        if (GUI.Button(new Rect(startX, startY, buttonWidth, buttonHeight), isRecording ? "Stop" : "Record"))
        {
            if(isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
            isRecording = !isRecording;
        }
    }

}
