using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayAudioAction : BaseAction
{
    public override ActionType ActionType => ActionType.PlayAudioAction;
    
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    public PlayAudioAction(AudioSource audioSource, AudioClip audioClip)
    {
        _audioSource = audioSource;
        _audioClip = audioClip;
    }
    
    public override async UniTask Excuse()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        
        await UniTask.Delay(TimeSpan.FromSeconds(_audioClip.length));
    }
}
