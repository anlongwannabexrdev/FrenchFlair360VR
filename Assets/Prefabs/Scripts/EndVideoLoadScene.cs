using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VideoEndLoadScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneName;
    public Canvas fadeCanvas;
    public Image fadeImage;
    public float fadeDuration = 2f;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        if (fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(false); 
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeToGrayAndLoadScene());
    }

    IEnumerator FadeToGrayAndLoadScene()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(true); 
        }

        float timer = 0f;
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(0.5f, 0.5f, 0.5f, 1);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}