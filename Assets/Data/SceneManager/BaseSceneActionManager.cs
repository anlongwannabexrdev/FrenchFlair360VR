using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseSceneActionManager : MonoBehaviour
{
    public List<BaseAction> actions = new List<BaseAction>();
    
    public virtual void GenerationAction()
    {
        
    }
    
    public virtual async UniTask ExcuseAction()
    {
        foreach (var action in actions)
        {
            await action.Excuse();
        }
    }
}
