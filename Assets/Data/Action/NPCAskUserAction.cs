using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class NPCAskUserAction : BaseAction
{
   [SerializeField]
   private ChatManager _chatManager;
   [SerializeField]
   private string _textToSpeak;
   [SerializeField]
   private List<UniTask> _tasks = new List<UniTask>();
   
   public NPCAskUserAction(ChatManager chatManager)
   {
      _chatManager = chatManager;

      _textToSpeak = string.Empty;
   }

   public NPCAskUserAction(ChatManager chatManager, string textToSpeak)
   {
      _chatManager = chatManager;

      _textToSpeak = textToSpeak;
   }

   private async UniTask ExcuseTextToSpeak()
   {
      Debug.LogWarning($"NPCSpeak {_textToSpeak}");

      await _chatManager.NpcController.NPCSpeak(_textToSpeak);
   }

   
   public override async UniTask Excuse()
   {
      if (!string.IsNullOrEmpty(_textToSpeak))
      {
         _tasks.Add(ExcuseTextToSpeak());
      }
      
      foreach (var task in _tasks)
      {
         await task;
      }
   }
}
