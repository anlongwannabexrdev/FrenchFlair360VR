using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData", order = 1)]
public class SceneData : ScriptableObject
{
    public List<VideoData> videoDatas = new List<VideoData>();

    public bool TryGetVideoData(string videoID, out VideoData videoData)
    {
        foreach (var video in videoDatas)
        {
            if (video.videoID == videoID)
            {
                videoData = video;
                return true;
            }
        }
        
        videoData = default;
        return true;
    }
}

[System.Serializable]
public class VideoData
{
    public string videoID;
    public QuestionData question = new QuestionData();

    public string nextVideoIDToPlay;
}

[System.Serializable]
public class QuestionData
{
    public string question;

    public string promptData;

    public AnswerOption option1 = new AnswerOption();
    public AnswerOption option2 = new AnswerOption();
    public AnswerOption option3 = new AnswerOption();
}

[System.Serializable]
public class AnswerOption
{
    public AnswerType Type;
    public AudioClip clipToPlay;
    public string sceneIDToChange;
}

public enum AnswerType
{
    NeutralResponse = 0,
    DisapprovingResponse = 1,
    CorrectAnswer = 2,
}
