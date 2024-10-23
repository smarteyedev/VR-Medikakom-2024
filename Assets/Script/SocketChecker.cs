using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketChecker : MonoBehaviour
{
    public XRSocketInteractor[] sockets; // Array berisi semua socket yang akan dicek
    public new ParticleSystem particleSystem; // Referensi ke Particle System
    public GameObject Perintah9;
    public GameObject Perintah10;
    public GameObject WarningHeatPanel;
    public GameObject DoneHeatPanel;


    private void Start()
    {
        // Memulai pengecekan socket secara berkala
        StartCoroutine(CheckSockets());
    }

    private IEnumerator CheckSockets()
    {
        while (true)
        {
            // Tunggu sejenak sebelum pengecekan ulang
            yield return new WaitForSeconds(1f);

            // Cek apakah semua socket sudah terisi
            bool allSocketsFilled = AreAllSocketsFilled();

            // Jika semua socket terisi, matikan particle system
            if (allSocketsFilled && particleSystem.isPlaying)
            {
                particleSystem.Stop(); // Mematikan Particle System
                Perintah9.SetActive(false);
                WarningHeatPanel.SetActive(false);
                Perintah10.SetActive(true);
                DoneHeatPanel.SetActive(true);

                yield return new WaitForSeconds(1f);
                DoneHeatPanel.SetActive(false); 

                yield break; // Keluar dari coroutine karena sudah selesai
            }
        }
    }

    private bool AreAllSocketsFilled()
    {
        // Loop melalui semua socket dan cek apakah ada yang belum terisi
        foreach (var socket in sockets)
        {
            if (!socket.hasSelection) // Jika socket tidak terisi
            {
                return false; // Ada socket yang masih kosong
            }
        }
        return true; // Semua socket terisi
    }
}
