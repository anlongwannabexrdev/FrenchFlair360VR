using System;
using NaughtyAttributes;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public ChatBot chatBot;

    public SceneData SceneData;
    public string videoID;
    public TextToSpeech TextToSpeech;

    public ChatManager ChatManager;


    [Button]
    public void StartAskUser()
    {
        if (SceneData.TryGetVideoData(videoID, out VideoData videoData))
        {
            TextToSpeech.SpeechText(videoData.question.question,(() =>
            {
                ChatManager.UserController.StartRecorder();
            }));
        }
    }

    public void StartAnswerUser(string userAnswer)
    {
        chatBot.SpeakToBot(userAnswer,UserAskBotCallback);
    }

    public void UserAskBotCallback(OpenMisa.ChatCompletion chatCompletion)
    {
        if (chatCompletion == null)
        {
            Debug.Log("Chat null");
            return;
        }
        
        Debug.Log(chatCompletion.GetMessageBot());
        
        TextToSpeech.SpeechText(chatCompletion.GetMessageBot(),null);
    }
}
