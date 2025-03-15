using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using Convai.Scripts.Runtime.Extensions;

public class SpeechToSpeech : MonoBehaviour
{
    private string apiKey = "sk_6cb7b6633da184efa4481329ccf50c17c522636d6a046b1a";
    private string apiUrl = "https://api.elevenlabs.io/v1/speech-to-speech/";
    private string voiceId = "EXAVITQu4vr4xnSDxMaL"; // Voice ID (chọn từ ElevenLabs)

    public AudioClip audioClipTest;

    public void Start()
    {
        if(audioClipTest != null)
        {
            Conversational(audioClipTest);
        }
    }

    public void Conversational(AudioClip audioClip)
    {
        byte[] audioData = WavUtility.FromAudioClip(audioClip);
        StartCoroutine(PostAudio(audioData));
    }

    IEnumerator PostAudio(byte[] audioData)
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        string requestUrl = apiUrl + voiceId;

        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", audioData, "recordedAudio.wav", "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(requestUrl, form))
        {
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("accept", "audio/mpeg");

            yield return request.SendWebRequest();

            stopwatch.Stop();
            Debug.Log($"Elevenlab STS: {stopwatch.ElapsedMilliseconds} ms");

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
                    UnityEngine.Debug.Log("Playing from ElevenLabs!");
                }
                else
                {
                    UnityEngine.Debug.LogError("Can't create AudioClip from response!");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Error on download AudioClip: " + www.error);
            }
        }
    }

}

internal static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip clip)
    {
        // Contains formatting information (sample frequency, channel number, etc.).
        int HEADER_SIZE = 44;

        // Lấy thông tin từ AudioClip
        int samples = clip.samples;
        int channels = clip.channels;
        int sampleRate = clip.frequency;

        // Lấy dữ liệu âm thanh từ AudioClip
        float[] audioData = new float[samples * channels];
        clip.GetData(audioData, 0);

        // Tính kích thước dữ liệu âm thanh (mỗi mẫu là 2 byte vì PCM 16-bit)
        int dataSize = samples * channels * 2;
        int fileSize = HEADER_SIZE + dataSize;

        // Tạo MemoryStream để chứa dữ liệu WAV
        using (MemoryStream memoryStream = new MemoryStream(fileSize))
        using (BinaryWriter writer = new BinaryWriter(memoryStream))
        {
            // Ghi header WAV
            WriteWavHeader(writer, sampleRate, channels, samples);

            // Ghi dữ liệu âm thanh
            WriteWavData(writer, audioData);

            // Trả về mảng byte
            return memoryStream.ToArray();
        }
    }

    private static void WriteWavHeader(BinaryWriter writer, int sampleRate, int channels, int samples)
    {
        writer.Write(new char[4] { 'R', 'I', 'F', 'F' }); // Chunk ID
        writer.Write(36 + samples * channels * 2); // Chunk Size (file size - 8)
        writer.Write(new char[4] { 'W', 'A', 'V', 'E' }); // Format

        // Subchunk 1: fmt
        writer.Write(new char[4] { 'f', 'm', 't', ' ' }); // Subchunk1 ID
        writer.Write(16); // Subchunk1 Size (16 cho PCM)
        writer.Write((short)1); // Audio Format (1 = PCM)
        writer.Write((short)channels); // Number of Channels
        writer.Write(sampleRate); // Sample Rate
        writer.Write(sampleRate * channels * 2); // Byte Rate (SampleRate * NumChannels * BitsPerSample/8)
        writer.Write((short)(channels * 2)); // Block Align (NumChannels * BitsPerSample/8)
        writer.Write((short)16); // Bits Per Sample

        // Subchunk 2: data
        writer.Write(new char[4] { 'd', 'a', 't', 'a' }); // Subchunk2 ID
        writer.Write(samples * channels * 2); // Subchunk2 Size (NumSamples * NumChannels * BitsPerSample/8)
    }

    private static void WriteWavData(BinaryWriter writer, float[] audioData)
    {
        // Chuyển đổi dữ liệu float thành PCM 16-bit
        foreach (float sample in audioData)
        {
            short intSample = (short)(sample * 32767); // Chuyển từ [-1, 1] sang [-32767, 32767]
            writer.Write(intSample);
        }
    }

}