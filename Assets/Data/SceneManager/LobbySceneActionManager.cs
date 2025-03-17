using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class LobbySceneActionManager : BaseSceneActionManager
{
    public AudioSource audioSource;
    public AudioClip audioClip;
   
    public ChangeSceneCmp changeSceneCmp;
    public string sceneToChange;

    private void Start()
    {
        GenerationAction();

        ExcuseAction().Forget();
    }

    [Button]
    public override void GenerationAction()
    {
        if (!AppManager.Instance.isFinishIntroScene)
        {
            // Play Audio
            PlayAudioAction playAudioAction = new PlayAudioAction(audioSource, audioClip);
            actions.Add(playAudioAction);
        
            // Change Scene
            ChangeSceneAction changeSceneAction = new ChangeSceneAction(changeSceneCmp,sceneToChange);
            actions.Add(changeSceneAction);

            DelayTimeAction delayTimeAction = new DelayTimeAction(2f);
            actions.Add(delayTimeAction);

            AppManager.Instance.isFinishIntroScene = true;
        }
    }
    
    [Button]
    public override async UniTask ExcuseAction()
    {
        await base.ExcuseAction();
    }
}
