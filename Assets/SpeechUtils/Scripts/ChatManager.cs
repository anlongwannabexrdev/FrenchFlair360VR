using DG.Tweening;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public UserController UserController;
    public NPCController NpcController;
}

public static class DOTweenExtensions
{
    public static Tweener DelayCall(this GameObject go, float delay, TweenCallback callback)
    {
        if (delay > 0)
        {
            return DOTween.To((t) => { }, 0, delay, delay).OnComplete(callback).SetId(go);
        }
        else
        {
            callback();
            return null;
        }
    }
}