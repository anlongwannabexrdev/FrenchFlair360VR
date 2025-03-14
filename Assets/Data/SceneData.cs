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
}
