using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmoothSocketInsert : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Kecepatan pergerakan objek (atur agar lebih rendah untuk pergerakan lebih lambat)
    private Transform targetObject; // Objek yang akan dimasukkan
    private Vector3 targetPosition; // Posisi socket
    private Quaternion targetRotation; // Rotasi socket

    private bool isMoving = false;

    // Method publik yang dipanggil dari event `Select Entered` di Inspector
    public void StartObjectMovement(XRBaseInteractable interactableObject)
    {
        targetObject = interactableObject.transform; // Mengambil objek yang masuk ke socket
        targetPosition = transform.position; // Posisi socket (lokasi dimana objek bergerak)
        targetRotation = transform.rotation; // Rotasi socket agar objek dapat berorientasi ke arah yang benar
        isMoving = true; // Memulai proses pergerakan objek
    }

    void Update()
    {
        // Jika objek sedang dalam proses perpindahan ke socket
        if (isMoving && targetObject != null)
        {
            // Gerakkan objek secara perlahan menggunakan MoveTowards
            targetObject.position = Vector3.MoveTowards(targetObject.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // Putar objek agar sesuai dengan orientasi socket
            targetObject.rotation = Quaternion.Slerp(targetObject.rotation, targetRotation, moveSpeed * Time.deltaTime);

            // Mengecek apakah objek sudah mencapai socket (dengan toleransi 0.01)
            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f && Quaternion.Angle(targetObject.rotation, targetRotation) < 1f)
            {
                // Jika sudah mencapai posisi dan rotasi yang diinginkan, hentikan perpindahan
                isMoving = false;
            }
        }
    }
}
