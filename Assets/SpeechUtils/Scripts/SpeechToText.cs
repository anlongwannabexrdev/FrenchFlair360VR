using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class SpeechToText : MonoBehaviour
{
    private string apiKey = "AIzaSyAoE9FrwmIzmclz0wOlxhpv-dSUEL727ok";
    private string apiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=";

    public AudioClip recordedClip;

    void Start()
    {
        SendAudio(AudioConverter.ConvertAudioClipToByteArray(recordedClip));
    }

    public void SendAudio(byte[] audioData)
    {
        StartCoroutine(PostAudio(audioData));
    }

    IEnumerator PostAudio(byte[] audioData)
    {
        string json = "{\"config\": {\"encoding\":\"LINEAR16\",\"sampleRateHertz\":16000,\"languageCode\":\"en-US\"},\"audio\": {\"content\": \"" + System.Convert.ToBase64String(audioData) + "\"}}";

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + apiKey, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Speech-to-Text Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
    }
}


public static class AudioConverter
{
    public static byte[] ConvertAudioClipToByteArray(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null!");
            return null;
        }

        // Get audio data as float array
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // Convert float samples to byte array (16-bit PCM)
        byte[] byteArray = new byte[samples.Length * 2]; // 2 bytes per sample (16-bit PCM)
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * short.MaxValue); // Scale float (-1 to 1) to short (-32768 to 32767)
            byteArray[i * 2] = (byte)(sample & 0xFF);
            byteArray[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
        }

        return byteArray;
    }
}