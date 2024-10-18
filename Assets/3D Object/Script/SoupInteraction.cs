using UnityEngine;

public class SoupInteraction : MonoBehaviour
{
    public GameObject soupPrefab;  // Referensi ke prefab sup
    private GameObject currentSoup; // Sup yang diinstansiasi ke spatula
    public GameObject spatula;  // Referensi ke spatula
    public Transform spatulaAttachPoint; // Titik sup menempel di spatula
    public Transform potTransform;  // Transform dari panci
    public Transform bowlTransform; // Transform dari mangkok
    public Transform bowlSpawnPoint; // Titik spawn untuk sup di mangkok (spawner)
    public float potRange = 0.5f;  // Jarak minimum antara spatula dan panci untuk memindahkan sup
    public float bowlRange = 0.5f; // Jarak minimum antara spatula dan mangkok untuk memindahkan sup ke mangkok
    private bool isSoupTaken = false;  // Status apakah sup sudah diambil

    void Start()
    {
        // Sup belum diambil, tidak diinstansiasi di spatula pada awalnya.
        currentSoup = null; // Sup tidak muncul di spatula pada awal permainan.
    }

    void Update()
    {
        // Cek jarak antara spatula dan panci jika sup belum diambil
        if (!isSoupTaken)
        {
            float distanceToPot = Vector3.Distance(spatula.transform.position, potTransform.position);
            if (distanceToPot <= potRange)
            {
                Debug.Log("Spatula mendekati panci, memindahkan sup.");
                MoveSoupToSpatula();  // Memindahkan sup ke spatula ketika dalam jarak potRange
            }
        }
        else
        {
            // Cek jarak antara spatula dan mangkok untuk memindahkan sup ke mangkok
            float distanceToBowl = Vector3.Distance(spatula.transform.position, bowlTransform.position);
            if (distanceToBowl <= bowlRange)
            {
                Debug.Log("Spatula mendekati mangkok, memindahkan sup ke mangkok.");
                MoveSoupToBowl();  // Memindahkan sup ke mangkok ketika dalam jarak bowlRange
            }
        }
    }

    private void MoveSoupToSpatula()
    {
        // Pastikan sup belum diambil sebelum memindahkannya ke spatula
        if (currentSoup == null)
        {
            // Instansiasi sup dari prefab ke spatula
            currentSoup = Instantiate(soupPrefab, spatulaAttachPoint.position, Quaternion.identity); 
            currentSoup.transform.rotation = spatulaAttachPoint.rotation;  // Sesuaikan rotasi sup dengan spatula
            currentSoup.transform.SetParent(spatulaAttachPoint); // Set sup sebagai anak dari spatula
            isSoupTaken = true;  // Tandai bahwa sup sudah diambil
            Debug.Log("Sup berhasil dipindahkan ke spatula.");
        }
    }

    private void MoveSoupToBowl()
    {
        // Pastikan sup sudah ada di spatula sebelum memindahkannya ke mangkok
        if (currentSoup != null)
        {
            // Pindahkan sup dari spatula ke mangkok dengan menggunakan titik spawn
            currentSoup.transform.SetParent(null);  // Hapus hubungan parent dengan spatula
            currentSoup.transform.position = bowlSpawnPoint.position;  // Posisikan sup ke titik spawn di mangkok
            currentSoup.transform.rotation = bowlSpawnPoint.rotation;  // Sesuaikan rotasi sup dengan rotasi spawn point
            currentSoup.transform.localScale = bowlSpawnPoint.localScale;  // Sesuaikan ukuran sup dengan skala spawn point
            Debug.Log("Sup berhasil dipindahkan ke mangkok.");
            currentSoup = null;  // Reset sup setelah dipindahkan ke mangkok
            isSoupTaken = false;  // Tandai bahwa sup tidak lagi di spatula
        }
    }
}
