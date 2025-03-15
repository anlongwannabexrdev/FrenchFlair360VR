using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public ChatManager chatManager;

    public RecorderV2 recorderV2;
    public SpeechToTextV2 speechToTextV2;

    public async UniTask StartRecored()
    {
        Debug.LogWarning($"UserController_StartRecored");

        await recorderV2.StartRecording();
    }

    public async UniTask<string> ConvertAudioToText(AudioClip audioClip)
    {
        await speechToTextV2.ConvertAsync(audioClip);
        
        Debug.LogWarning($"UserController_ConvertAudioToText_{speechToTextV2.finalText}");

        return speechToTextV2.finalText;
    }
}
