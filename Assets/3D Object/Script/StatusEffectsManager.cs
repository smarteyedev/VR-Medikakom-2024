using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsManager : MonoBehaviour
{
    public GameObject energizedEffect;
    public bool isEnergized;

    public float duration = 5f; // Set default duration

    private Vector3 originalScale;
    private CircularProgressBar progressBar; // Referensi ke progress bar

    private void Start()
    {
        // Simpan skala asli dari objek energizedEffect
        originalScale = energizedEffect.transform.localScale;

        // Dapatkan referensi ke progress bar dan matikan
        progressBar = energizedEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>();
        progressBar.gameObject.SetActive(false); // Matikan progress bar di awal

        energizedEffect.transform.localScale = Vector3.zero;
        StartEnergizedEffect(duration);
    }

    public void StartEnergizedEffect(float duration)
    {
        if (isEnergized)
        {
            Debug.Log("Efek sudah aktif, tidak memulai lagi.");
            return; // Menghindari memulai efek yang sama
        }

        isEnergized = true;
        energizedEffect.SetActive(true);
        PopUpEffect(duration);
    }

    private void PopUpEffect(float duration)
    {
        energizedEffect.transform.localScale = Vector3.zero;
        LeanTween.scale(energizedEffect, originalScale, 1.0f).setEaseOutSine().setOnComplete(() =>
        {
            // Aktifkan progress bar ketika pop-up selesai
            progressBar.gameObject.SetActive(true);
            progressBar.ActiveCountdown(duration); // Mulai countdown

            // Mulai coroutine untuk mengakhiri efek setelah durasi
            StartCoroutine(EndEnergizedEffect(duration));
        });
    }

    IEnumerator EndEnergizedEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        isEnergized = false;
        PopOutEffect();
    }

    private void PopOutEffect()
    {
        LeanTween.scale(energizedEffect, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() =>
        {
            energizedEffect.SetActive(false);
            progressBar.gameObject.SetActive(false); // Matikan progress bar setelah efek selesai
        });
    }
}
