using System.Collections;
using UnityEngine;

public class PemotonganSayur : MonoBehaviour
{
    public GameObject wortelUtuh; // Objek wortel utuh
    public GameObject[] prefabWortelTerpotong; // Array untuk berbagai jenis potongan wortel
    public Transform posisiSpawn; // Lokasi spawn untuk wortel terpotong
    public float waktuJeda = 1.0f; // Waktu jeda dalam detik sebelum wortel diganti

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang berinteraksi adalah pisau
        if (other.CompareTag("Pisau"))
        {
            // Mulai coroutine untuk menambahkan jeda sebelum mengganti wortel
            StartCoroutine(GantiWortelDenganJeda());
        }
    }

    // Coroutine untuk menambahkan jeda
    IEnumerator GantiWortelDenganJeda()
    {
        // Tunggu selama waktuJeda sebelum eksekusi lanjut
        yield return new WaitForSeconds(waktuJeda);

        // Nonaktifkan wortel utuh
        wortelUtuh.SetActive(false);

        // Pilih potongan wortel secara acak
        int randomIndex = Random.Range(0, prefabWortelTerpotong.Length);
        GameObject selectedWortel = prefabWortelTerpotong[randomIndex];

        // Spawn potongan wortel yang dipilih di posisi yang sama
        Instantiate(selectedWortel, posisiSpawn.position, posisiSpawn.rotation);
    }
}