using Cysharp.Threading.Tasks;

[System.Serializable]
public class BaseAction
{
    public virtual ActionType ActionType => ActionType.None;
    public virtual async UniTask Excuse()
    {
        
    }
}

public enum ActionType
{
    None = -1,
    
    PlayAudioAction = 0,
    
    NPCAskUserAction = 1,
    
    NPCResponesUserAction = 2,
    
    UserAnswerNPCAction = 3,
    
    ChangeSceneAction = 4,
    
    DelayTimeAction = 5,
    
    CommonAction = 6,
    
    MergeAction = 7,
}
