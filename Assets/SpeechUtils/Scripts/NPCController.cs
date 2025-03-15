using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public ChatManager ChatManager;

    public TextToSpeechV2 textToSpeechV2;

    public ChatBotV2 chatBotV2;

    public async UniTask NPCSpeak(string textToSpeak)
    {
        Debug.LogWarning($"NPCController_NPCSpeak_{textToSpeak}");
        
        await textToSpeechV2.SpeechText(textToSpeak,null);
    }

    public async UniTask<OpenMisa.ChatBotContent> AnswerUser(string userAnswer,string promntData)
    { 
        Debug.LogWarning($"NPCController_AnswerUser_{userAnswer}");

       await chatBotV2.SpeakToBot(userAnswer, promntData);

       return chatBotV2.chatBotContent;
    }
}
