using System;
using Cysharp.Threading.Tasks;

public class CommonAction : BaseAction
{
    private Action _callback;

    public CommonAction(Action callback)
    {
        _callback = callback;
    }
    
    public override ActionType ActionType => ActionType.CommonAction;
    public override async UniTask Excuse()
    {
        _callback?.Invoke();
        
        await UniTask.CompletedTask;
    }
}

public class CommonActionV2 : BaseAction
{
    private Func<UniTask> _actionFactory;

    public CommonActionV2(Func<UniTask> actionFactory)
    {
        _actionFactory = actionFactory;
    }
    
    public override async UniTask Excuse()
    {
        await _actionFactory();
    }
}