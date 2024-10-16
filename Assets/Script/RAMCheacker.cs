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
    public GameObject SocketToActive;

    // Audio source to play SFX
    public AudioSource audioSource;
    public AudioClip socketFilledSFX;

    // Object to enable XR Grab Interactable when all sockets are filled
    public GameObject objectToEnableGrab; // Add this line

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
            SocketToActive.SetActive(true);

            // Deactivate the panels to hide
            panelToHide1.SetActive(false);
            panelToHide2.SetActive(false);

            // Play the SFX
            PlaySFX();

            // Activate XR Grab Interactable for the object
            ActivateXRGrab();

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

    // Function to activate XR Grab Interactable for a specific object
    void ActivateXRGrab()
    {
        if (objectToEnableGrab != null)
        {
            // Ensure the object has XRGrabInteractable component and enable it
            XRGrabInteractable grabInteractable = objectToEnableGrab.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = true;
            }
            else
            {
                Debug.LogWarning("The object does not have an XRGrabInteractable component.");
            }
        }
    }
}
