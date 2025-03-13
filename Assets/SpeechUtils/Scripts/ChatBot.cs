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
    private static readonly string apiKey = "cWsTC69JOYww85MTy0xV7Ii5k79onrpn";
    private static readonly string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    public TextToSpeech textToSpeech;

    void Start()
    {
        StartCoroutine(GetMistralResponse("Xin chào, bạn khỏe không?"));
    }

    IEnumerator GetMistralResponse(string prompt)
    {
        var requestData = new
        {
            model = "mistral-7b",
            messages = new[]
            {
                new { role = "assistant", content = "You are NPC, answer not over 10 word." },
                new { role = "user", content = prompt }
            },
            temperature = 0.7
        };


        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData));

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

            Debug.Log("Mistral Response: " + response);

            response = (from item in JObject.Parse(response)["choices"] select item!["message"]!["content"]!).FirstOrDefault().ToString();

            Debug.Log("Mistral Response: " + response);

            textToSpeech.SpeechText(response);
        }
        else
        {
            Debug.LogError("Lỗi: " + request.error);
        }
    }
}
