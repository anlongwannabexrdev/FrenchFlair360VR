using System;
using System.Collections;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] private int maxTimeToRecord = 30;
    [SerializeField] private SpeechToText speechToText;
    
    private AudioClip recordedClip;
    private const int sampleRate = 16000;

    public Action<AudioClip> onStopRecord;
    
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
        
        onStopRecord?.Invoke(recordedClip);
    }
}
