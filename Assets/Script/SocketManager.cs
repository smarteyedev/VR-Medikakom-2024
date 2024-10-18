using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class PCAssemblyManager : MonoBehaviour
{
    [System.Serializable]
    public class AssemblyStep
    {
        public string componentName; // Nama komponen (misalnya CPU, RAM)
        public XRSocketInteractor socket; // Socket yang sesuai untuk komponen
        public GameObject component; // Objek komponen yang akan dipasang
        public GameObject infoPanel1; // Panel informasi pertama
        public GameObject infoPanel2; // Panel informasi kedua
        public GameObject spawner; // GameObject yang berfungsi sebagai spawner
    }

    public List<AssemblyStep> assemblySteps = new List<AssemblyStep>(); // List untuk menyimpan langkah-langkah perakitan
    public GameObject notificationPanel; // Panel notifikasi yang muncul jika urutan salah
    public float notificationDuration = 2f; // Durasi notifikasi

    public GameObject successAudioObject; // GameObject yang berisi AudioSource untuk SFX sukses
    public GameObject errorAudioObject; // GameObject yang berisi AudioSource untuk SFX error

    private int currentStep = 0; // Langkah saat ini (step 1, step 2, dst.)

    void Start()
    {
        // Sembunyikan semua panel informasi di awal
        foreach (var step in assemblySteps)
        {
            step.infoPanel1.SetActive(false);
            step.infoPanel2.SetActive(false);
        }

        // Sembunyikan panel notifikasi
        notificationPanel.SetActive(false);
    }

    void Update()
    {
        if (currentStep < assemblySteps.Count)
        {
            // Cek apakah komponen di langkah saat ini sudah terpasang
            if (assemblySteps[currentStep].socket.interactablesSelected.Count > 0 &&
                assemblySteps[currentStep].socket.GetOldestInteractableSelected().transform == assemblySteps[currentStep].component.transform)
            {
                // Jika komponen sesuai dengan urutan, mainkan SFX success dan lanjutkan ke langkah berikutnya
                PlaySFX(successAudioObject);
                currentStep++;

                // Pastikan langkah berikutnya tidak melebihi jumlah assemblySteps
                if (currentStep < assemblySteps.Count)
                {
                    // Tampilkan panel informasi untuk langkah selanjutnya
                    ShowInfoPanels(currentStep);
                }
            }

            // Cek apakah komponen yang dipasang di luar urutan
            for (int i = currentStep + 1; i < assemblySteps.Count; i++)
            {
                if (assemblySteps[i].socket.interactablesSelected.Count > 0 &&
                    assemblySteps[i].socket.GetOldestInteractableSelected().transform == assemblySteps[i].component.transform)
                {
                    // Jika komponen dipasang di luar urutan, kembalikan ke spawner, mainkan SFX error
                    PlaySFX(errorAudioObject);
                    ReturnComponentToSpawner(i);
                    StartCoroutine(ShowNotification());
                }
            }
        }
    }

    void ReturnComponentToSpawner(int stepIndex)
    {
        // Kembalikan komponen ke posisi spawner
        GameObject spawner = assemblySteps[stepIndex].spawner;

        // Pindahkan komponen ke posisi dan rotasi spawner
        assemblySteps[stepIndex].component.transform.position = spawner.transform.position;
        assemblySteps[stepIndex].component.transform.rotation = spawner.transform.rotation; // Mengatur rotasi ke rotasi spawner

        // Dapatkan interaksi yang sedang terjadi
        var selectedInteractable = assemblySteps[stepIndex].socket.GetOldestInteractableSelected();

        if (selectedInteractable != null)
        {
            // Gunakan interactionManager untuk memaksa interaksi keluar
            assemblySteps[stepIndex].socket.interactionManager.SelectExit(assemblySteps[stepIndex].socket, selectedInteractable);
        }
    }

    IEnumerator ShowNotification()
    {
        // Tampilkan panel notifikasi
        notificationPanel.SetActive(true);

        // Tunggu selama 2 detik
        yield return new WaitForSeconds(notificationDuration);

        // Sembunyikan panel notifikasi
        notificationPanel.SetActive(false);
    }

    void ShowInfoPanels(int stepIndex)
    {
        // Jika ada langkah sebelumnya, sembunyikan panel informasinya
        if (stepIndex > 0)
        {
            assemblySteps[stepIndex - 1].infoPanel1.SetActive(false);
            assemblySteps[stepIndex - 1].infoPanel2.SetActive(false);
        }

        // Tampilkan panel informasi untuk langkah yang sesuai
        assemblySteps[stepIndex].infoPanel1.SetActive(true);
        assemblySteps[stepIndex].infoPanel2.SetActive(true);
    }

    // Fungsi untuk memutar SFX dari GameObject yang memiliki AudioSource
    void PlaySFX(GameObject audioObject)
    {
        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
