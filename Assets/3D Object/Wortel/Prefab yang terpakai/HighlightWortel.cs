using UnityEngine;

public class HighlightWortel : MonoBehaviour
{
    public Color highlightColor = Color.yellow; // Warna highlight saat pisau mendekat
    private Color originalColor; // Warna asli wortel
    private Renderer rend; // Renderer dari wortel

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color; // Simpan warna asli
    }

    // Ketika pisau memasuki area box collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            Highlight(true); // Aktifkan highlight
        }
    }

    // Ketika pisau keluar dari area box collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            Highlight(false); // Matikan highlight
        }
    }

    // Fungsi untuk mengaktifkan/mematikan highlight
    void Highlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            rend.material.color = highlightColor; // Ubah ke warna highlight
        }
        else
        {
            rend.material.color = originalColor; // Kembalikan ke warna asli
        }
    }
}
