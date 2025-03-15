using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class NPCResponesUserAction : BaseAction
{
    private ChatManager _chatManager;
    [SerializeField]
    private UserAnswerNPCAction _userAnswer;
    [SerializeField]
    private string _prompt;
   
    public NPCResponesUserAction(ChatManager chatManager, UserAnswerNPCAction userAnswer,string prompt)
    {
        _chatManager = chatManager;
        _userAnswer = userAnswer;
        _prompt = prompt;
    }
    
    public override async UniTask Excuse()
    {
        var respone=await _chatManager.NpcController.AnswerUser(_userAnswer.speakToTText, _prompt);

        if (respone != null)
        {
           await _chatManager.NpcController.NPCSpeak(respone.GetMessageBot());
        }
    }
}
