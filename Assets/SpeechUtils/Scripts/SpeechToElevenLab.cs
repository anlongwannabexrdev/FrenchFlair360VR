using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpeechToElevenLab : MonoBehaviour
{
    public event System.Action<string> onDetectSpeech;

    private string apiKey = "AIzaSyAoE9FrwmIzmclz0wOlxhpv-dSUEL727ok";
    private string apiUrl = "https://api.elevenlabs.io/v1/speech-to-text";

    public void Convert(AudioClip audioClip)
    {
        byte[] audioData = ConvertAudioClipToByteArray(audioClip);
        StartCoroutine(PostAudio(audioData));
    }

    IEnumerator PostAudio(byte[] audioData)
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        
        var form = new WWWForm();
        form.AddBinaryData("file", audioData, "file1.wav", "audio/wav");
        form.AddField("model_id", "scribe_v1");

        using var request = UnityWebRequest.Post(apiUrl, form);
        request.SetRequestHeader("Content-Type", "multipart/form-data");
        request.SetRequestHeader("xi-api-key", apiKey);
        yield return request.SendWebRequest();

        stopwatch.Stop();
        Debug.Log($"Elevenlab STT: {stopwatch.ElapsedMilliseconds} ms");

        if (request.result == UnityWebRequest.Result.Success)
        {
            string userText = request.downloadHandler.text;
            Debug.Log("User said: " + userText);

            onDetectSpeech?.Invoke(userText);
        }
        else
        {
            Debug.LogError("STT Error: " + request.error);
        }
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