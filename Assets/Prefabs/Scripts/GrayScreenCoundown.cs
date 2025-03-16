using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ScreenGrayCountdown : MonoBehaviour
{
    public Image overlayImage;
    public Text countdownText;
    public Image circularTimer; 
    public float countdownTime = 30f;
    public VideoPlayer videoPlayer;
    public Button exitButton;
    public GameObject pauseImage;
    public GameObject circular_Timer;

    private void Start()
    {
        overlayImage.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        pauseImage.SetActive(false);
        circular_Timer.SetActive(false);

        circularTimer.fillAmount = 0f; 

        circularTimer.gameObject.SetActive(false);
        countdownText.color = Color.white;
    }

    public void StartCountdown()
    {
        overlayImage.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        circularTimer.gameObject.SetActive(true);
        circularTimer.fillAmount = 1f; 
        overlayImage.color = new Color(0, 0, 0, 0.7f);

        if (videoPlayer != null)
        {
            videoPlayer.Pause();
        }

        pauseImage.SetActive(true);
        circular_Timer.SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            circularTimer.fillAmount = timer / countdownTime; 
            if(timer <= 5)
            {
                countdownText.color = Color.red;
            }
            yield return new WaitForSeconds(1f);
            timer--;
        }

        exitButton.gameObject.SetActive(true);
        circularTimer.fillAmount = 0f; 
        circularTimer.gameObject.SetActive(false);
        //circular_Timer.SetActive(false);
    }
}
