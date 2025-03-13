using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ChatBot : MonoBehaviour
{
    public event System.Action<string> onRespond;

    private static readonly string apiKey = "cWsTC69JOYww85MTy0xV7Ii5k79onrpn";
    private static readonly string apiUrl = "https://api.mistral.ai/v1/chat/completions";

    public void AskToBot(string prompt)
    {
        StartCoroutine(GetMistralResponse(prompt));
    }

    IEnumerator GetMistralResponse(string prompt)
    {
        var requestData = new
        {
            model = "open-mistral-7b",
            messages = new[]
            {
                new { role = "assistant", content = "You are NPC, the answer just less than 10 words" },
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
            Debug.Log("Bot Response: " + response);
            try
            {
                response = (from item in JObject.Parse(response)["choices"] select item!["message"]!["content"]!).FirstOrDefault().ToString();
                onRespond?.Invoke(response);
            }
            catch
            {
                Debug.LogError("Can't convert reponse from AI");
            }

        }
        else
        {
            Debug.LogError("Bot Error: " + request.error);
        }
    }
}
