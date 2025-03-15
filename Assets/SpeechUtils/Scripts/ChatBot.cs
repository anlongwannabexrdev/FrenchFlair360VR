using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ChatBot : MonoBehaviour
{
    public System.Action<string> onRespond;

    private static readonly string apiKey = "cWsTC69JOYww85MTy0xV7Ii5k79onrpn";
    private static readonly string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    public string videoID;
    public string myAnswer;
    public SceneData SceneData;

    public void AskToBot(string prompt)
    {
        StartCoroutine(GetMistralResponse(prompt));
    }

    public void SpeakToBot(string myAnswer)
    {
        this.myAnswer = myAnswer;
        
        if (SceneData.TryGetVideoData(videoID, out VideoData videoData))
        {
            StartCoroutine(GetMistralResponse(videoData.question.promptData));
        }
    }

    IEnumerator GetMistralResponse(string prompt)
    {
        var requestData = new
        {
            model = "open-mistral-7b",
            messages = new[]
            {
                new { role = "assistant", content = prompt },
                new { role = "user", content = myAnswer }
            },
            temperature = 0.7
        };


        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData));

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        stopwatch.Stop();
        UnityEngine.Debug.Log($"AI Model: {stopwatch.ElapsedMilliseconds} ms");

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            UnityEngine.Debug.Log("Bot Response: " + response);
            //completion = JsonConvert.DeserializeObject<OpenMisa.ChatCompletion>(response);
            try
            {
                response = (from item in JObject.Parse(response)["choices"] select item!["message"]!["content"]!).FirstOrDefault().ToString();
                onRespond?.Invoke(response);
            }
            catch
            {
                UnityEngine.Debug.LogError("Can't convert reponse from AI");
            }

        }
        else
        {
            UnityEngine.Debug.LogError("Bot Error: " + request.error);
        }
    }
}

public class OpenMisa
{
    [System.Serializable]
    public class ChatBotContent
    {
        public string content;
        public string feedback;
        public string title;
        public ChatBotContent(string content)
        {
            this.content = content;
        }

        public string GetMessageBot()
        {
            feedback = "Not Found";
            int feedbackIndex = content.IndexOf("Feedback:");
            if (feedbackIndex != -1)
            {
                feedback = content.Substring(feedbackIndex + 9).Trim(); // Extract everything after "Feedback:" (length 9)
            }

            return feedback;
        }

        public string GetOption()
        {
            title = content.Contains(":") ? content.Split(':')[0] : "Not Found";
            return title;
        }
    }
}