using UnityEngine;
using UnityEngine.UI; // Untuk menggunakan UI panel
using System.Collections;

public class FloatingRobot : MonoBehaviour
{
    public float amplitude = 0.5f; // Ketinggian floating
    public float frequency = 1f;    // Kecepatan floating

    // Posisi awal dari robot
    Vector3 startPos;

    // Referensi ke panel UI
    public GameObject panel1; // Panel pertama
    public GameObject panel2; // Panel kedua
    public GameObject panel3; // Panel ketiga
    public GameObject panel4; // Panel keempat

    // Referensi ke objek yang akan muncul saat panel 3 ditampilkan
    public GameObject object1; // Objek pertama
    public GameObject object2; // Objek kedua
    public GameObject object3; // Objek ketiga

    // Referensi ke AudioSource untuk SFX
    public AudioSource audioSource;
    public AudioClip panelChangeSFX; // SFX untuk pergantian panel

    void Start()
    {
        // Simpan posisi awal robot
        startPos = transform.position;

        // Mulai animasi floating menggunakan LeanTween
        StartFloating();

        // Tampilkan panel pertama
        ShowPanel(panel1);
    }

    void StartFloating()
    {
        // Buat gerakan naik
        LeanTween.moveY(gameObject, startPos.y + amplitude, frequency)
            .setEaseInOutSine()  // Menggunakan easing sinusoidal untuk gerakan naik turun
            .setLoopPingPong();   // Looping bolak-balik (naik dan turun)
    }

    void ShowPanel(GameObject panel)
    {
        panel.SetActive(true); // Tampilkan panel
        PlayPanelChangeSFX(); // Mainkan SFX saat panel ditampilkan
        
        // Memunculkan objek saat panel3 ditampilkan
        if (panel == panel3)
        {
            ShowObjects(); // Tampilkan objek saat panel3 muncul
        }

        // Mengatur coroutine untuk menyembunyikan panel kecuali panel 4
        if (panel != panel4)
        {
            StartCoroutine(HidePanelAfterDelay(panel, 3f)); // Hide panel setelah 3 detik
        }
    }

    IEnumerator HidePanelAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay); // Tunggu selama delay
        panel.SetActive(false); // Sembunyikan panel
        
        // Tampilkan panel berikutnya
        if (panel == panel1)
        {
            ShowPanel(panel2);
        }
        else if (panel == panel2)
        {
            ShowPanel(panel3);
        }
        else if (panel == panel3)
        {
            ShowPanel(panel4); // Tampilkan panel4
            // Tidak ada tindakan untuk menyembunyikan panel4
        }
        // Tidak ada else untuk panel4, sehingga tetap aktif
    }

    void ShowObjects()
    {
        object1.SetActive(true); // Tampilkan objek pertama
        object2.SetActive(true); // Tampilkan objek kedua
        object3.SetActive(true); // Tampilkan objek ketiga
    }

    void PlayPanelChangeSFX()
    {
        if (audioSource != null && panelChangeSFX != null)
        {
            audioSource.PlayOneShot(panelChangeSFX); // Mainkan efek suara
        }
    }
}
