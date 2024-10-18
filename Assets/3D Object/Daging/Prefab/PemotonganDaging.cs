using System.Collections;
using UnityEngine;

public class PemotonganDaging : MonoBehaviour
{
    public GameObject dagingUtuh; // Objek daging utuh
    public GameObject prefabDagingTerpotong; // Prefab potongan daging
    public Transform posisiSpawn; // Lokasi spawn untuk daging terpotong
    public float waktuJeda = 1.0f; // Waktu jeda dalam detik sebelum daging diganti
    public int potonganIndex = 2; // Indeks potongan daging (yang akan dipakai di BubbleChatManager)

    // Tambahkan boolean untuk menandai potongan terakhir
    public bool adalahPotonganTerakhir; // Ceklis di Inspector untuk potongan terakhir

    private static int currentMeatPartsSpawned = 0; // Counter untuk potongan daging yang ter-*spawn*

    // AudioSource dan AudioClip untuk daging
    private AudioSource audioSource;
    public AudioClip sfxDaging;  // SFX untuk daging (bisa di-set di Inspector per clone)

    // Event untuk bubble chat daging
    public static event System.Action<int> potonganDagingEvent; // Event untuk bubble chat
    public static event System.Action potonganDagingSelesaiEvent; // Event untuk pemotongan selesai

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            if (sfxDaging != null)
            {
                audioSource.clip = sfxDaging;
                audioSource.Play();
            }

            StartCoroutine(GantiDagingDenganJeda());
        }
    }

    IEnumerator GantiDagingDenganJeda()
    {
        yield return new WaitForSeconds(waktuJeda);

        dagingUtuh.SetActive(false);

        // Spawn potongan daging baru
        Instantiate(prefabDagingTerpotong, posisiSpawn.position, posisiSpawn.rotation);

        // Increment counter setiap kali potongan daging baru ter-*spawn*
        currentMeatPartsSpawned++;

        // Trigger event untuk bubble chat daging
        potonganDagingEvent?.Invoke(potonganIndex);

        // Cek jika ini adalah potongan terakhir
        if (adalahPotonganTerakhir)
        {
            // Trigger event pemotongan selesai
            potonganDagingSelesaiEvent?.Invoke();
        }
    }

    // Reset counter (Opsional, bisa dipakai untuk reset permainan)
    public static void ResetMeatCutting()
    {
        currentMeatPartsSpawned = 0;
    }
}
