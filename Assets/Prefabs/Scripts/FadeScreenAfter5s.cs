using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

public class FadeScreenAfter5s : MonoBehaviour
{
    public Image fadeImage;
    public string nextSceneName;
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
        Invoke("ActivateFadeImage", 5f);
    }

    void ActivateFadeImage()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0.3f, 0.3f, 0.3f, 0);
        }
        isFading = true;
    }

    void Update()
    {
        if (isFading && fadeImage != null)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / 2f);
            fadeImage.color = new Color(0.3f, 0.3f, 0.3f, alpha);

            if (alpha >= 1f)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
