using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutEffect : MonoBehaviour
{
    public Image image; // Referensi ke komponen Image
    public Button fadeOutButton; // Referensi ke tombol
    public float fadeDuration = 1.0f; // Durasi fade out

    private void Start()
    {
        // Menambahkan listener pada tombol
        fadeOutButton.onClick.AddListener(OnFadeOutButtonClicked);
    }

    public void OnFadeOutButtonClicked()
    {
        // Memulai coroutine untuk fade out dan fade in
        StartCoroutine(FadeOutIn());
    }

    private IEnumerator FadeOutIn()
    {
        // Fade Out
        yield return FadeTo(1); // Fade ke alpha 1 (sepenuhnya terlihat)

        // Tunggu selama 2 detik
        yield return new WaitForSeconds(2f); // Menunggu 2 detik

        // Fade In
        yield return FadeTo(0); // Fade kembali ke alpha 0 (transparan)
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color color = image.color; // Mengambil warna saat ini dari Image
        float startAlpha = color.a; // Menyimpan alpha awal
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime; // Menambah waktu
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration); // Menghitung alpha baru
            image.color = color; // Menerapkan warna baru ke Image
            yield return null; // Menunggu frame berikutnya
        }

        // Mengatur alpha ke target setelah selesai
        color.a = targetAlpha; 
        image.color = color;
    }
}
