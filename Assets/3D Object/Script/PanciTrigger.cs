using UnityEngine;

public class PanciTrigger : MonoBehaviour
{
    public PengelolaPanci pengelolaPanci; // Referensi ke PengelolaPanci

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BahanMasak"))
        {
            Debug.Log("Bahan Masak terdeteksi oleh trigger Panci: " + other.gameObject.name);

            // Cek apakah bahan masak ini belum ada di list di PengelolaPanci
            if (!pengelolaPanci.bahanMasakDiPanci.Contains(other.gameObject))
            {
                // Tambahkan bahan masak ke dalam list di PengelolaPanci
                pengelolaPanci.bahanMasakDiPanci.Add(other.gameObject);

                // Cek apakah semua bahan masak sudah masuk ke panci
                if (pengelolaPanci.SemuaBahanMasakAda())
                {
                    Debug.Log("Semua bahan sudah terdeteksi, menutup panci...");
                    // Panggil fungsi TutupPanci() jika tutup sudah dipasang
                    if (pengelolaPanci.socketTutup.selectTarget != null)
                    {
                        StartCoroutine(pengelolaPanci.TutupPanci());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BahanMasak"))
        {
            Debug.Log("Bahan Masak keluar dari trigger Panci: " + other.gameObject.name);

            // Hapus bahan masak dari list di PengelolaPanci jika keluar dari panci
            if (pengelolaPanci.bahanMasakDiPanci.Contains(other.gameObject))
            {
                pengelolaPanci.bahanMasakDiPanci.Remove(other.gameObject);
            }
        }
    }
}
