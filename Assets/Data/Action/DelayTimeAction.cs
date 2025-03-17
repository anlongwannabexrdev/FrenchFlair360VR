using System;
using Cysharp.Threading.Tasks;

public class DelayTimeAction : BaseAction
{
    public override ActionType ActionType => ActionType.DelayTimeAction;

    private float _delayTime;

    public DelayTimeAction(float delayTime)
    {
        _delayTime = delayTime;
    }
    
    public override async UniTask Excuse()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));
    }
}