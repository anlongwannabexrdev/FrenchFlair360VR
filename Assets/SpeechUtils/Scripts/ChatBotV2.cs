using Cysharp.Threading.Tasks; // Add UniTask namespace
using System;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ChatBotV2 : MonoBehaviour
{
    public System.Action<string> onRespond;

    private static readonly string apiKey = "cWsTC69JOYww85MTy0xV7Ii5k79onrpn";
    private static readonly string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    public string myAnswer;
    public string promptDataTest;
    public OpenMisa.ChatBotContent chatBotContent = new OpenMisa.ChatBotContent(string.Empty);

    [Button]
    public void TestData()
    {
        SpeakToBot(myAnswer, promptDataTest);
    }

    public void AskToBot(string prompt)
    {
        GetMistralResponse(prompt).Forget(); // Fire-and-forget for simplicity
    }

    public async UniTask SpeakToBot(string myAnswer,string promptData)
    {
        this.myAnswer = myAnswer;

        await GetMistralResponse(promptData);
    }

    private async UniTask GetMistralResponse(string prompt)
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

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData));

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            await request.SendWebRequest().ToUniTask(); // Convert to UniTask

            stopwatch.Stop();
            Debug.Log($"AI Model: {stopwatch.ElapsedMilliseconds} ms");

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                
                Debug.Log("Bot Response: " + response);
                JObject parsedJson = JObject.Parse(response);
                string content = (string)parsedJson["choices"][0]["message"]["content"];
                chatBotContent = new OpenMisa.ChatBotContent(content);
                try
                {
                    response = (from item in JObject.Parse(response)["choices"]
                                select item!["message"]!["content"]!).FirstOrDefault().ToString();
                    onRespond?.Invoke(response);
                }
                catch
                {
                    Debug.LogError("Can't convert response from AI");
                }
            }
            else
            {
                Debug.LogError("Bot Error: " + request.error);
            }
        }
    }
}