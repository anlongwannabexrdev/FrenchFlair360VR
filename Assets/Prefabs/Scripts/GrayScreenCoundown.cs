using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ScreenGrayCountdown : MonoBehaviour
{
    public Image overlayImage;
    public TextMeshProUGUI countdownText;
    public float countdownTime = 30f;
    public VideoPlayer videoPlayer;
    public Button exitButton;
    public GameObject pauseImage; 

    private void Start()
    {
        overlayImage.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        pauseImage.SetActive(false);
    }

    public void StartCountdown()
    {
        overlayImage.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        overlayImage.color = new Color(0, 0, 0, 0.5f); 

        if (videoPlayer != null)
        {
            videoPlayer.Pause(); 
        }

        pauseImage.SetActive(true); 
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }

        exitButton.gameObject.SetActive(true); 
    }
}
