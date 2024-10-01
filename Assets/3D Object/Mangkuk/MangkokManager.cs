using UnityEngine;
using System.Collections.Generic;

public class MangkokManager : MonoBehaviour
{
    public List<GameObject> wortelList; // List untuk berbagai jenis wortel yang akan dimasukkan
    public GameObject mangkok; // Objek mangkok

    private Rigidbody mangkokRb; // Rigidbody mangkok
    private BoxCollider mangkokCollider; // Collider mangkok

    private List<BoxCollider> wortelColliders = new List<BoxCollider>(); // Collider untuk wortel
    private List<bool> isWortelInsideList = new List<bool>(); // Mengecek apakah wortel sudah di dalam mangkok

    private void Start()
    {
        // Ambil komponen Rigidbody dan Collider dari mangkok
        mangkokRb = mangkok.GetComponent<Rigidbody>();
        mangkokCollider = mangkok.GetComponent<BoxCollider>();

        // Inisialisasi untuk setiap wortel dalam list
        foreach (GameObject wortel in wortelList)
        {
            BoxCollider wortelCollider = wortel.GetComponent<BoxCollider>();
            wortelColliders.Add(wortelCollider);
            isWortelInsideList.Add(false); // Semua wortel belum ada di dalam mangkok di awal
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Loop melalui setiap wortel
        for (int i = 0; i < wortelList.Count; i++)
        {
            if (other.gameObject == wortelList[i] && !isWortelInsideList[i])
            {
                // Nonaktifkan Collider wortel sementara agar tidak mengganggu mangkok
                wortelColliders[i].enabled = false;

                // Set bool bahwa wortel sekarang ada di dalam mangkok
                isWortelInsideList[i] = true;

                // Posisikan wortel di dalam mangkok
                wortelList[i].transform.position = mangkok.transform.position + new Vector3(0, 0.1f * (i + 1), 0); // Posisikan berurutan
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Loop melalui setiap wortel
        for (int i = 0; i < wortelList.Count; i++)
        {
            if (other.gameObject == wortelList[i])
            {
                // Aktifkan kembali Collider wortel setelah dikeluarkan
                wortelColliders[i].enabled = true;

                // Set bool bahwa wortel sudah keluar dari mangkok
                isWortelInsideList[i] = false;
            }
        }
    }

    private void Update()
    {
        // Mengecek apakah mangkok dilempar, dan jika iya, Collider wortel tetap aktif
        if (mangkokRb.velocity.magnitude > 0.1f)
        {
            foreach (BoxCollider wortelCollider in wortelColliders)
            {
                wortelCollider.enabled = true;
            }
        }
    }
}
