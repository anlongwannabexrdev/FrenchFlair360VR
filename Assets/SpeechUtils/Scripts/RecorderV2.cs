using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class RecorderV2 : MonoBehaviour
{
    [SerializeField] private int maxTimeToRecord = 30;
    
    public AudioClip recordedClip;
    private const int sampleRate = 16000;
    private bool isRecording;

    public Action<AudioClip> onStopRecord;

    public async UniTask StartRecording()
    {
        if (isRecording)
        {
            Debug.LogWarning("Already recording!");
            return;
        }

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        isRecording = true;
        string deviceName = Microphone.devices[0];
        recordedClip = Microphone.Start(deviceName, false, maxTimeToRecord, sampleRate);

        if (recordedClip == null)
        {
            Debug.LogError("Failed to start recording!");
            isRecording = false;
            return;
        }

        Debug.Log("Recording started...");

        // Wait until recording stops
        await UniTask.WaitWhile(() => Microphone.IsRecording(deviceName));

        Debug.Log("Recording stopped.");
        isRecording = false;

        if (recordedClip != null && recordedClip.length > 0)
        {
            onStopRecord?.Invoke(recordedClip);
        }
        else
        {
            Debug.LogWarning("No audio data captured!");
        }
    }

    public void StopRecording()
    {
        if (!isRecording) return;
        Microphone.End(null);
        isRecording = false;
    }

}