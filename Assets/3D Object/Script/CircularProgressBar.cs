using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircularProgressBar : MonoBehaviour
{
    public Image fillImage; // Image untuk mengisi progress
    private Coroutine countdownCoroutine;

    // Mengaktifkan countdown pada progress bar
    public void ActiveCountdown(float countdownTime)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(Countdown(countdownTime));
    }

    private IEnumerator Countdown(float countdownTime)
    {
        float timer = 0f;
        while (timer < countdownTime)
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / countdownTime);
            yield return null; // Tunggu hingga frame berikutnya
        }

        fillImage.fillAmount = 1f; // Set ke penuh saat selesai
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        fillImage.fillAmount = 0f; // Reset ke kosong
    }
}
