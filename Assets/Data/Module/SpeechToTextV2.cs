using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks; // Add this for UniTask

public class SpeechToTextV2 : MonoBehaviour
{
    public System.Action<string> onDetectSpeech;

    private string apiKey = "AIzaSyAoE9FrwmIzmclz0wOlxhpv-dSUEL727ok";
    private string apiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=";
    public string finalText;

    private bool _isConverting;

    public async UniTask ConvertAsync(AudioClip audioClip)
    {
        if (_isConverting)
        {
            return;
        }
        
        finalText = string.Empty;
        byte[] audioData = ConvertAudioClipToByteArray(audioClip);
        if (audioData != null)
        {
            _isConverting = true;
            await PostAudioAsync(audioData);
        }
    }

    private async UniTask PostAudioAsync(byte[] audioData)
    {
        string json = "{\"config\": {\"encoding\":\"LINEAR16\",\"sampleRateHertz\":16000,\"languageCode\":\"en-US\"},\"audio\": {\"content\": \"" + System.Convert.ToBase64String(audioData) + "\"}}";

        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        using var request = new UnityWebRequest(apiUrl + apiKey, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send request and await completion
        await request.SendWebRequest().ToUniTask();

        stopwatch.Stop();
        Debug.Log($"Google STT: {stopwatch.ElapsedMilliseconds} ms");

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            try
            {
                var result = JObject.Parse(response)["results"][0]["alternatives"][0]["transcript"].ToString();
                onDetectSpeech?.Invoke(result);

                finalText = result;
            }
            catch
            {
                Debug.Log("Can't convert Speech to text");
            }
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }

        _isConverting = false;
    }

    private static byte[] ConvertAudioClipToByteArray(AudioClip clip)
    {
        if (clip == null)
        {
            UnityEngine.Debug.LogError("AudioClip is null!");
            return null;
        }

        // Get audio data as float array
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // Convert float samples to byte array (16-bit PCM)
        byte[] byteArray = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * short.MaxValue);
            byteArray[i * 2] = (byte)(sample & 0xFF);
            byteArray[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
        }

        return byteArray;
    }
}