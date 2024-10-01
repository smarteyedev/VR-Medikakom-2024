using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoveController : MonoBehaviour
{
    public ParticleSystem stoveParticle;  // Particle system for the stove
    public XRGrabInteractable grabInteractable;  // XR Grab Interactable component
    public AudioSource stoveSFX;  // Audio source for the stove sound effect
    public Transform player;  // The player's transform
    public float maxDistance = 20f;  // Maximum distance for sound to play
    public float minDistance = 5f;   // Minimum distance for maximum sound

    private bool isSFXPlaying = false;
    private bool isParticlePlaying = false;  // Flag to track particle state

    void Start()
    {
        // Make sure the particle and sound are stopped initially
        stoveParticle.Stop();
        stoveSFX.Stop();

        // Register events for when the object is grabbed or released
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    // When the object is grabbed
    public void OnGrabbed(SelectEnterEventArgs args)
    {
        if (isParticlePlaying)
        {
            // If particle is already playing, stop both particle and sound
            stoveParticle.Stop();
            stoveSFX.Stop();
            isSFXPlaying = false;
            isParticlePlaying = false;
            Debug.Log("Particle and SFX stopped on re-grab.");
        }
        else
        {
            // Play particle and sound if not already playing
            stoveParticle.Play();
            stoveSFX.Play();
            isSFXPlaying = true;
            isParticlePlaying = true;
            Debug.Log("Particle and SFX started on grab.");
        }
    }

    // When the object is released
    public void OnReleased(SelectExitEventArgs args)
    {
        // Keep particle and sound playing when released
        Debug.Log("Object released, particle and SFX stay active.");
    }

    void Update()
    {
        if (isSFXPlaying)
        {
            // Calculate the distance between the player and the stove
            float distance = Vector3.Distance(player.position, transform.position);

            // Control the volume of the SFX based on the player's distance
            if (distance <= minDistance)
            {
                stoveSFX.volume = 1f;  // Full volume when close
            }
            else if (distance >= maxDistance)
            {
                stoveSFX.volume = 0f;  // No sound when too far
            }
            else
            {
                // Gradually decrease volume between min and max distance
                float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
                stoveSFX.volume = 1f - normalizedDistance;  // Volume decreases as distance increases
            }
        }
    }
}
