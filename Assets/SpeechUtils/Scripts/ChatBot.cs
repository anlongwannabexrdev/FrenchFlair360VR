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

    public OpenMisa.ChatCompletion completion;
    public Action<OpenMisa.ChatCompletion> askBotCallback;

    public void AskToBot(string prompt)
    {
        StartCoroutine(GetMistralResponse(prompt));
    }

    public void SpeakToBot(string myAnswer,Action<OpenMisa.ChatCompletion> callback)
    {
        this.myAnswer = myAnswer;
        askBotCallback = callback;
        
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
            completion = JsonConvert.DeserializeObject<OpenMisa.ChatCompletion>(response);
            try
            {
                response = (from item in JObject.Parse(response)["choices"] select item!["message"]!["content"]!).FirstOrDefault().ToString();
                onRespond?.Invoke(response);
                askBotCallback?.Invoke(completion);
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
    public class ChatCompletion
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }

        public string GetMessageBot()
        {
            string fullText = Choices[0].Message.Content;

// Find the index of "Feedback: " and extract everything after it
            int startIndex = fullText.IndexOf("Feedback: ") + "Feedback: ".Length;
            string feedback = fullText.Substring(startIndex);

            Console.WriteLine(feedback);
            return feedback;
        }
    }
    
    [System.Serializable]
    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
    }
    
    [System.Serializable]
    public class Message
    {
        public string Role { get; set; }
        public object ToolCalls { get; set; }  // Using object since it can be null
        public string Content { get; set; }
    }
    
    [System.Serializable]
    public class Usage
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
        public int CompletionTokens { get; set; }
    }
}