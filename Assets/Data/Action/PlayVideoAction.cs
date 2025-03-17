using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Video;

public class PlayVideoAction : BaseAction
{
    private VideoPlayer _videoPlayer;
    private VideoClip _videoClip;

    public PlayVideoAction(VideoPlayer videoPlayer, VideoClip videoClip)
    {
        _videoPlayer = videoPlayer;
        _videoClip = videoClip;
    }
    
    public override async UniTask Excuse()
    {
        _videoPlayer.clip = _videoClip;
        _videoPlayer.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(_videoPlayer.length));
    }
}