using System.Collections;
using UnityEngine;

public class ObjectActivationManager : MonoBehaviour
{
    // Object references
    public GameObject Perintah7; // Object pertama
    public GameObject Perintah8; // Object kedua

    public GameObject PokeButton; // Poke Button

    // Time delay between switching objects
    public float delayTime = 1f;

    void Update()
    {
        // Check if object1 is active
        if (Perintah7.activeSelf)
        {
            // Start coroutine to deactivate object1 and activate object2 after a delay
            StartCoroutine(SwitchObjects());
        }
    }

    IEnumerator SwitchObjects()
    {
        // Deactivate object1 after delay
        yield return new WaitForSeconds(delayTime);
        
        // Deactivate object1
        Perintah7.SetActive(false);

        // Activate object2
        Perintah8.SetActive(true);
        PokeButton.SetActive(true);

        // Stop the coroutine to avoid repeating the activation/deactivation process
        StopCoroutine(SwitchObjects());
    }
}
