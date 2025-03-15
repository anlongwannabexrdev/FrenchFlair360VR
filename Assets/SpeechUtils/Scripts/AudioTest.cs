using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using NaughtyAttributes;

public class AudioTest : MonoBehaviour
{
    public AudioSource AudioSource; // Assign in Inspector

    [Button]
    public void Play()
    {
        AudioSource.Play();
    }

    void Start()
    {
        // Test with a known working URL or ElevenLabs API
      //  StartCoroutine(PlayAudio("https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"));
        // For ElevenLabs, uncomment and configure:
        // StartCoroutine(FetchElevenLabsAudio("Hello, this is a test!"));
    }

    IEnumerator PlayAudio(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    Debug.Log("Clip loaded - Length: " + clip.length);
                    AudioSource.spatialBlend = 0f; // 2D audio
                    AudioSource.clip = clip;
                    AudioSource.mute = false;
                    AudioSource.volume = 1.0f;
                    AudioSource.Play();
                    Debug.Log("Playing: " + AudioSource.isPlaying);
                }
                else
                {
                    Debug.LogError("Clip is null!");
                }
            }
            else
            {
                Debug.LogError("Download failed: " + www.error);
            }
        }
    }
}