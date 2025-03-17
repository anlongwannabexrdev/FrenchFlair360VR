using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CoolDownCycleTimeCmp : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 30f;
    public GameObject circular_Timer;
    public Image circularTimer; 
    
    public async UniTask StartCountdown()
    {
        gameObject.SetActive(true);
        countdownText.color = Color.white;
        
        countdownText.gameObject.SetActive(true);
        circularTimer.gameObject.SetActive(true);
        circularTimer.fillAmount = 1f;

        circular_Timer.SetActive(true);

        await CountdownRoutine();
    }

    private async UniTask CountdownRoutine()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            circularTimer.fillAmount = timer / countdownTime;
            if (timer <= 5)
            {
                countdownText.color = Color.red;
            }
            await UniTask.Delay(1000);
            timer--;
        }
        circularTimer.fillAmount = 0f;
        circularTimer.gameObject.SetActive(false);
    }
}
