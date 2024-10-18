using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class PengelolaPanci : MonoBehaviour
{
    [System.Serializable]
    public class SoupItem
    {
        public GameObject prefabSoup;
        public Transform spawnPoint;  
    }

    public SoupItem[] soupItems;  
    public XRSocketInteractor socketTutup; 
    public float waktuMasak = 10f; 
    public ParticleSystem steamParticleSystem; 
    public AudioSource sfxBell; 
    public Collider triggerPanci; 
    public CircularProgressBar progressBar; 
    public GameObject container; 

    public bool tutupDitutup = false; 
    public List<GameObject> bahanMasakDiPanci = new List<GameObject>(); 
    private bool soupSudahDiSpawn = false; 
    private Coroutine masakCoroutine; 
    public bool isStoveOn = false; 

    private void Start()
    {
        if (steamParticleSystem != null)
        {
            steamParticleSystem.gameObject.SetActive(false); // Pastikan sistem partikel dimatikan di awal
        }
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
        if (container != null)
        {
            container.SetActive(false); // Pastikan container dinonaktifkan di awal
            container.transform.localScale = Vector3.zero; // Set scale to zero for pop-up effect
        }
    }

    private void Update()
    {
        if (socketTutup.interactablesSelected.Count > 0 && !tutupDitutup)
        {
            StartCoroutine(TutupPanci());
        }
        else if (socketTutup.interactablesSelected.Count == 0 && tutupDitutup)
        {
            BukaTutupPanci();
        }
    }

    public IEnumerator TutupPanci()
    {
        tutupDitutup = true;

        if (SemuaBahanMasakAda() && !soupSudahDiSpawn)
        {
            if (!isStoveOn)
            {
                Debug.Log("Kompor perlu dinyalakan agar bisa dimasak");
                yield break; 
            }

            Debug.Log("Semua bahan masak ada di dalam panci, mulai memasak.");
            masakCoroutine = StartCoroutine(Memasak());
            yield return masakCoroutine;
        }
        else
        {
            Debug.Log("Semua bahan masak ada di dalam panci, tidak perlu di spawn kembali.");
        }

        if (steamParticleSystem != null)
        {
            steamParticleSystem.Stop(); // Pastikan partikel berhenti saat menutup panci
        }
    }

    private IEnumerator Memasak()
    {
        float timer = 0f;

        if (container != null)
        {
            container.SetActive(true);
            LeanTween.scale(container, Vector3.one, 0.5f).setEaseOutBack(); 
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.ActiveCountdown(waktuMasak);
        }

        // Nyalakan partikel asap saat memasak dimulai
        if (steamParticleSystem != null)
        {
            steamParticleSystem.Play(); // Nyalakan asap saat memasak dimulai
            Debug.Log("Partikel asap dinyalakan.");
        }

        while (timer < waktuMasak)
        {
            timer += Time.deltaTime;

            if (!tutupDitutup)
            {
                Debug.Log("Memasak dibatalkan karena tutup diangkat.");
                progressBar?.StopCountdown();
                yield break; 
            }

            yield return null; 
        }

        foreach (GameObject bahan in bahanMasakDiPanci)
        {
            bahan.SetActive(false);
        }

        foreach (SoupItem soupItem in soupItems)
        {
            GameObject soupInstance = Instantiate(soupItem.prefabSoup, soupItem.spawnPoint.position, soupItem.spawnPoint.rotation);
            soupInstance.tag = "Soup"; 
        }

        // Setelah memasak, pastikan partikel asap dinyalakan jika makanan sudah jadi
        if (steamParticleSystem != null)
        {
            steamParticleSystem.Play(); // Nyalakan partikel asap setelah memasak
            Debug.Log("Partikel asap dinyalakan setelah memasak.");
        }
        
        if (sfxBell != null)
        {
            sfxBell.Play();
        }

        soupSudahDiSpawn = true;

        yield return new WaitForSeconds(1f); 

        if (progressBar != null)
        {
            progressBar?.StopCountdown();
        }

        if (container != null)
        {
            LeanTween.scale(container, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() => container.SetActive(false)); 
        }
    }

    public bool SemuaBahanMasakAda()
    {
        return bahanMasakDiPanci.Count > 0; 
    }

    public void BukaTutupPanci()
    {
        tutupDitutup = false;

        if (masakCoroutine != null)
        {
            StopCoroutine(masakCoroutine);
            masakCoroutine = null;
            Debug.Log("Memasak dihentikan.");
        }

        progressBar?.StopCountdown();
        
        // Nyalakan partikel asap hanya jika makanan sudah jadi
        if (steamParticleSystem != null && soupSudahDiSpawn)
        {
            steamParticleSystem.gameObject.SetActive(true); // Aktifkan GameObject partikel
            steamParticleSystem.Play(); // Nyalakan partikel asap saat panci dibuka
            Debug.Log("Partikel asap dinyalakan saat panci dibuka.");
        }
        else
        {
            Debug.Log("Partikel asap tidak dinyalakan karena makanan belum matang.");
        }

        Debug.Log("Tutup panci diangkat.");
    }

    public void ResetCookingTime()
    {
        soupSudahDiSpawn = false; 
        if (masakCoroutine != null)
        {
            StopCoroutine(masakCoroutine); 
        }

        progressBar?.StopCountdown();

        if (SemuaBahanMasakAda() && isStoveOn)
        {
            Debug.Log("Memulai proses memasak.");
            masakCoroutine = StartCoroutine(Memasak());
        }
        else
        {
            Debug.Log("Bahan masak belum lengkap atau kompor tidak dinyalakan.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BahanMasak"))
        {
            if (!bahanMasakDiPanci.Contains(other.gameObject))
            {
                bahanMasakDiPanci.Add(other.gameObject);
                other.gameObject.SetActive(false); 
                Debug.Log("Bahan masak ditambahkan ke panci.");
            }
        }
    }
}
