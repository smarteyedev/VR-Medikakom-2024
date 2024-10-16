using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UniversalSocketManager : MonoBehaviour
{
    // Durasi delay sebelum menonaktifkan BoxCollider (misalnya 0.5 detik)
    public float delayBeforeDisable = 0.5f;

    // Method ini dipanggil saat objek dipasang ke socket (dihubungkan ke event Select Entered)
    public void OnObjectPlaced(SelectEnterEventArgs args)
    {
        // Mengambil XRGrabInteractable dari objek yang dipasang
        XRGrabInteractable grabInteractable = args.interactableObject as XRGrabInteractable;

        if (grabInteractable != null)
        {
            // Mulai coroutine untuk menunggu sebelum menonaktifkan BoxCollider
            StartCoroutine(DisableColliderAfterDelay(grabInteractable));
        }
    }

    // Coroutine untuk menunggu sebelum menonaktifkan BoxCollider
    private IEnumerator DisableColliderAfterDelay(XRGrabInteractable grabInteractable)
    {
        // Menunggu selama beberapa detik
        yield return new WaitForSeconds(delayBeforeDisable);

        // Mendapatkan BoxCollider dari objek yang dipasang
        BoxCollider boxCollider = grabInteractable.GetComponent<BoxCollider>();

        // Menonaktifkan BoxCollider jika ada
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            Debug.Log("BoxCollider dinonaktifkan pada objek: " + grabInteractable.gameObject.name);
        }
        else
        {
            Debug.LogWarning("BoxCollider tidak ditemukan pada objek: " + grabInteractable.gameObject.name);
        }
    }
}
