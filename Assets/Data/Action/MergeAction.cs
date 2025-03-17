using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class MergeAction : BaseAction
{
    public List<BaseAction> actions = new List<BaseAction>();

    public MergeAction(List<BaseAction> actions)
    {
        this.actions = actions;
    }
    
    public override async UniTask Excuse()
    {
        List<UniTask> listActions = new List<UniTask>();
        foreach (var action in actions)
        {
            listActions.Add(action.Excuse());
        }
        
        await UniTask.WhenAll(listActions);
    }
}