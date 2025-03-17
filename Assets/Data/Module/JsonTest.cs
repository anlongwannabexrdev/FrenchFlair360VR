using System.Text.RegularExpressions;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public string input;

    public string outPut;
    public string outPutOption;
    public string outPutFeedback;
    
    [Button]
    public void Test()
    {
        // Your JSON string
        string jsonString = input;

        JObject parsedJson = JObject.Parse(jsonString);
        string content = (string)parsedJson["choices"][0]["message"]["content"];

        outPut = content;
        // Extract title
        string title = content.Contains(":") ? content.Split(':')[0] : "Not Found";

        // Extract feedback
        string feedback = "Not Found";
        int feedbackIndex = content.IndexOf("Feedback:");
        if (feedbackIndex != -1)
        {
            feedback = content.Substring(feedbackIndex + 9).Trim(); // Extract everything after "Feedback:" (length 9)
        }

        // Output results
        outPutOption = title;
        outPutFeedback = feedback;
    }
}
