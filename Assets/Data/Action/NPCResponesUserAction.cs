using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class NPCResponesUserAction : BaseAction
{
    public override ActionType ActionType => ActionType.NPCResponesUserAction;
    
    private ChatManager _chatManager;
    [SerializeField]
    private UserAnswerNPCAction _userAnswer;
    [SerializeField]
    private string _prompt;

    private VideoData _videoData;
    public NPCResponesUserAction(ChatManager chatManager, UserAnswerNPCAction userAnswer,string prompt,VideoData videoData)
    {
        _chatManager = chatManager;
        _userAnswer = userAnswer;
        _prompt = prompt;
        _videoData = videoData;
    }
    
    public override async UniTask Excuse()
    {
        var respone=await _chatManager.NpcController.AnswerUser(_userAnswer.speakToTText, _prompt);

        if (respone != null)
        {
           await _chatManager.NpcController.PlayNPCVoice(respone,_videoData);
        }
    }
}
