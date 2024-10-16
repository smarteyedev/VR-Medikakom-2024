using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // Pengaturan kustomisasi
    public float amplitude = 0.5f; // Ketinggian floating
    public float frequency = 1f;   // Kecepatan floating

    // Posisi awal dari object
    Vector3 startPos;

    void Start()
    {
        // Simpan posisi awal object
        startPos = transform.position;
    }

    void Update()
    {
        // Hitung gerakan naik turun menggunakan fungsi sinus
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;

        // Update posisi object
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
