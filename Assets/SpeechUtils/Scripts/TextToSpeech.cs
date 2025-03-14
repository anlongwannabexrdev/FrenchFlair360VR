using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System.IO;
using Convai.Scripts.Runtime.Extensions;

public class TextToSpeech : MonoBehaviour
{
    private string apiKey = "sk_6cb7b6633da184efa4481329ccf50c17c522636d6a046b1a";
    private string apiUrl = "https://api.elevenlabs.io/v1/text-to-speech/";
    private string voiceId = "EXAVITQu4vr4xnSDxMaL"; // Voice ID (ElevenLabs)
    
    public void SpeechText(string textToConvert)
    {
        StartCoroutine(GenerateSpeech(textToConvert));
    }

    IEnumerator GenerateSpeech(string textToConvert)
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        
        // Create payload
        string jsonPayload = "{\"text\":\"" + textToConvert + "\",\"model_id\":\"eleven_multilingual_v2\",\"voice_settings\":{\"stability\":0.5,\"similarity_boost\":0.7}}";
        
        byte[] postData = Encoding.UTF8.GetBytes(jsonPayload);

        var request = new UnityWebRequest(apiUrl + voiceId, "POST")
        {
            uploadHandler = new UploadHandlerRaw(postData),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("xi-api-key", apiKey);
        request.SetRequestHeader("accept", "audio/mpeg");

        yield return request.SendWebRequest();

        stopwatch.Stop();
        Debug.Log($"Elevenlabs TTS: {stopwatch.ElapsedMilliseconds} ms");

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] audioBytes = request.downloadHandler.data;

            // Save mp3 file temporarily so that Unity can read
            string tempFilePath = Path.Combine(Application.persistentDataPath, "tempAudio.mp3");
            File.WriteAllBytes(tempFilePath, audioBytes);

            yield return StartCoroutine(LoadAudioClip(tempFilePath));
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    IEnumerator LoadAudioClip(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    AudioSource audioSource = gameObject.GetOrAddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.Play();
                    Debug.Log("Playing from ElevenLabs!");
                }
                else
                {
                    Debug.LogError("Can't create AudioClip from response!");
                }
            }
            else
            {
                Debug.LogError("Error on download AudioClip: " + www.error);
            }
        }
    }
}