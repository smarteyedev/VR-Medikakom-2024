using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public GameObject bar;
    public int time;
    public GameObject timeUpMessage; // Tambahkan ini untuk menampilkan pesan setelah animasi selesai

    void Start()
    {
        timeUpMessage.SetActive(false); // Pastikan pesan tidak terlihat di awal
        AnimateBar();
    }

    public void AnimateBar()
    {
        // Gunakan LeanTween.scaleX() untuk mengubah skala pada sumbu X
        LeanTween.scaleX(bar, 1, time).setOnComplete(ShowMessage);
    }

    void ShowMessage()
    {
        timeUpMessage.SetActive(true); // Tampilkan pesan setelah animasi selesai
    }
}
