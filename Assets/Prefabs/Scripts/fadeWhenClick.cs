using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fadeWhenClick : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public string sceneToLoad;
    public GameObject button;
    public GameObject objectToHide;

    private void Start()
    {
        fadeImage.color = new Color(1, 1, 1, 0);
    }

    public void StartFade()
    {
        if (button != null)
            button.SetActive(false);

        if (objectToHide != null)
            objectToHide.SetActive(false);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
