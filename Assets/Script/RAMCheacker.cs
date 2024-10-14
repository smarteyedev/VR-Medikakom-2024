using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketCheckerWithSFX : MonoBehaviour
{
    // Array of XRSocketInteractors to check
    public XRSocketInteractor[] sockets;

    // Panels to show or hide
    public GameObject panelToShow1;
    public GameObject panelToShow2;
    public GameObject panelToHide1;
    public GameObject panelToHide2;

    // Audio source to play SFX
    public AudioSource audioSource;
    public AudioClip socketFilledSFX;

    // Flag to ensure SFX is played only once
    private bool sfxPlayed = false;

    void Update()
    {
        // Check if all sockets are filled
        if (AreAllSocketsFilled() && !sfxPlayed)
        {
            // Activate the panels to show
            panelToShow1.SetActive(true);
            panelToShow2.SetActive(true);

            // Deactivate the panels to hide
            panelToHide1.SetActive(false);
            panelToHide2.SetActive(false);

            // Play the SFX
            PlaySFX();

            // Set flag to true to avoid repeating SFX
            sfxPlayed = true;
        }
    }

    // Function to check if all sockets are filled
    bool AreAllSocketsFilled()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            if (!socket.hasSelection)
            {
                // If any socket is not filled, return false
                return false;
            }
        }
        // All sockets are filled, return true
        return true;
    }

    // Function to play the SFX
    void PlaySFX()
    {
        if (audioSource != null && socketFilledSFX != null)
        {
            audioSource.PlayOneShot(socketFilledSFX);
        }
    }
}
