using UnityEngine;
using UnityEngine.UI; // Pastikan untuk mengimpor namespace UI
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

    // Button untuk memulai urutan script
    public Button startButton; // Button untuk memulai urutan

    void Start()
    {
        // Simpan posisi awal robot
        startPos = transform.position;

        // Mulai animasi floating menggunakan LeanTween
        StartFloating();

        // Tambahkan listener ke button
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    void StartFloating()
    {
        // Buat gerakan naik
        LeanTween.moveY(gameObject, startPos.y + amplitude, frequency)
            .setEaseInOutSine()  // Menggunakan easing sinusoidal untuk gerakan naik turun
            .setLoopPingPong();   // Looping bolak-balik (naik dan turun)
    }

    void OnStartButtonClicked()
    {
        StartCoroutine(StartScriptSequence()); // Jalankan urutan script
    }

    IEnumerator StartScriptSequence()
    {
        // Tampilkan panel pertama setelah 2 detik
        yield return ShowPanelAfterDelay(panel1, 2f);
    }

    IEnumerator ShowPanelAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay); // Tunggu selama delay
        ShowPanel(panel); // Tampilkan panel setelah delay
    }

    void ShowPanel(GameObject panel)
    {
        panel.SetActive(true); // Tampilkan panel
        AnimatePanelScale(panel, Vector3.zero, Vector3.one, 0.5f); // Animasi scale up

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
        AnimatePanelScale(panel, Vector3.one, Vector3.zero, 0.5f); // Animasi scale down

        // Sembunyikan panel setelah animasi selesai
        yield return new WaitForSeconds(0.5f); // Tunggu selama animasi scale down
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

    void AnimatePanelScale(GameObject panel, Vector3 fromScale, Vector3 toScale, float duration)
    {
        panel.transform.localScale = fromScale; // Set scale awal
        LeanTween.scale(panel, toScale, duration).setEase(LeanTweenType.easeInOutBack); // Animasi scale
    }
}
