using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public string currentVideo;
    public SceneData sceneData;
    public List<BaseAction> actions = new List<BaseAction>();
    public ChatManager chatManager;
    [Button]
    public void GenerationActionForIntro()
    {
        
    }
    
    [Button]
    public void GenerationActionForScene1()
    {
        if (!sceneData.TryGetVideoData(currentVideo, out VideoData videoData))
        {
            return;
        }
        
        actions.Clear();
        // Play Video
        
        // 
        
        // NPC ask question for User
        NPCAskUserAction npcAskUserAction = new NPCAskUserAction(chatManager,videoData.question.question);
        actions.Add(npcAskUserAction);
        
        // User Answer NPC
        UserAnswerNPCAction userAnswerNpcAction = new UserAnswerNPCAction(chatManager);
        actions.Add(userAnswerNpcAction);
        
        // NPC response to User
        NPCResponesUserAction npcResponesUserAction =
            new NPCResponesUserAction(chatManager, userAnswerNpcAction, videoData.question.promptData,videoData);
        
        actions.Add(npcResponesUserAction);
    }

    [Button]
    public void ExcuseActionData()
    {
        ExcuseAction().Forget();
    }

    public async UniTask ExcuseAction()
    {
        foreach (var action in actions)
        {
            await action.Excuse();
        }
    }
}
