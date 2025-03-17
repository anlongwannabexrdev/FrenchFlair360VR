using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class UserAnswerNPCAction : BaseAction
{
    public override ActionType ActionType => ActionType.UserAnswerNPCAction;
    
    private ChatManager _chatManager;
    public AudioClip audioClip;
    public string speakToTText;
    public UserAnswerNPCAction(ChatManager chatManager)
    {
        _chatManager = chatManager;
    }
    
    public override async UniTask Excuse()
    {
        await _chatManager.UserController.StartRecored();

        audioClip = _chatManager.UserController.recorderV2.recordedClip;

        speakToTText = await _chatManager.UserController.ConvertAudioToText(audioClip);
    }
}
