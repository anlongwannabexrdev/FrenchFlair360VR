using System;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class BaseAction 
{
    protected Func<bool> isFinish;
    
    public virtual async UniTask Excuse()
    {
        
    }

    public async UniTask Finish()
    {
        await UniTask.WaitUntil(isFinish);
    }
}
