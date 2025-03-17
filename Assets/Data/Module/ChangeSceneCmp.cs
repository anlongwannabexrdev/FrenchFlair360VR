using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class ChangeSceneCmp : MonoBehaviour
{
    public Image fadeImage;        // The UI Image to fade
    public float fadeDuration = 1.5f; // Duration of the fade in seconds

    public bool autoLoadSccene;
    public string sceneToLoad;
    public float delayTimeToload;

    private void Start()
    {
        if (autoLoadSccene)
        {
            gameObject.DelayCall(delayTimeToload, (() =>
            {
                StartFade(sceneToLoad).Forget();
            }));
        }
    }

    [Button]
    private void TestLoadScene()
    {
        StartFade(sceneToLoad).Forget();
    }

    public async UniTask StartFade(string sceneToLoad)
    {
        fadeImage.gameObject.SetActive(true);
        // Fade the image to fully opaque using DOTween
        await fadeImage.DOFade(1f, fadeDuration).AsyncWaitForCompletion();
        
        SceneManager.LoadScene(sceneToLoad);
    }
    
}
