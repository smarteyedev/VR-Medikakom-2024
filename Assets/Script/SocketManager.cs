using Seville;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PCAssemblyManager : MonoBehaviour
{
    [System.Serializable]
    public class AssemblyStep
    {
        public string componentName;
        public SESocketInteractor socket;
        public GameObject component;
        public GameObject infoPanel1;
        public GameObject infoPanel2;
        public GameObject spawner;
        [HideInInspector] public Rigidbody componentRb;
        [HideInInspector] public XRGrabInteractable grabInteractable;
    }

    public List<AssemblyStep> assemblySteps = new List<AssemblyStep>();
    public GameObject notificationPanel;
    public float notificationDuration = 2f;

    public GameObject successAudioObject;
    public GameObject errorAudioObject;
    public float returnDelay = 2f;

    private int currentStep = 0;
    private bool componentPlacedCorrectly = false;

    void Start()
    {
        foreach (var step in assemblySteps)
        {
            step.infoPanel1.SetActive(false);
            step.infoPanel2.SetActive(false);

            step.socket.selectEntered.AddListener((args) => OnComponentPlaced(args, step));
            step.socket.selectExited.AddListener((args) => OnComponentRemoved(args, step));

            step.componentRb = step.component.GetComponent<Rigidbody>();
            if (step.componentRb == null)
            {
                step.componentRb = step.component.AddComponent<Rigidbody>();
            }

            step.grabInteractable = step.component.GetComponent<XRGrabInteractable>();
            if (step.grabInteractable != null)
            {
                step.grabInteractable.selectExited.AddListener((args) => OnComponentReleased(args, step));
            }
        }

        notificationPanel.SetActive(false);

        if (assemblySteps.Count > 0)
        {
            ShowInfoPanels(currentStep);
        }
    }

    private void OnComponentPlaced(SelectEnterEventArgs args, AssemblyStep step)
    {
        if (assemblySteps[currentStep] == step)
        {
            PlaySFX(successAudioObject);
            componentPlacedCorrectly = true;
            currentStep++;

            if (currentStep < assemblySteps.Count)
            {
                ShowInfoPanels(currentStep);
            }
        }
        else
        {
            PlaySFX(errorAudioObject);
            componentPlacedCorrectly = false;
            StartCoroutine(ReturnComponentToSpawner(step, returnDelay));
            StartCoroutine(ShowNotification());
        }
    }

    private void OnComponentRemoved(SelectExitEventArgs args, AssemblyStep step)
    {
        if (currentStep > 0 && assemblySteps[currentStep - 1] == step)
        {
            componentPlacedCorrectly = false;
            currentStep--;
            ShowInfoPanels(currentStep);
        }
    }

    private void OnComponentReleased(SelectExitEventArgs args, AssemblyStep step)
    {
        // Jika objek dilepaskan dari grab, mulai coroutine untuk mengembalikan objek setelah 2 detik jika tidak masuk socket
        StartCoroutine(CheckReturnToSpawner(step));
    }

    private IEnumerator CheckReturnToSpawner(AssemblyStep step)
    {
        yield return new WaitForSeconds(returnDelay);

        // Pastikan objek tidak di-grab dan tidak berada di dalam socket sebelum mengembalikannya ke spawner
        if (step.grabInteractable.isSelected == false && !step.socket.hasSelection)
        {
            StartCoroutine(ReturnComponentToSpawner(step, 0f));
        }
    }

    IEnumerator ReturnComponentToSpawner(AssemblyStep step, float delay)
    {
        yield return new WaitForSeconds(delay);

        var selectedInteractable = step.socket.GetOldestInteractableSelected();

        if (selectedInteractable != null)
        {
            step.socket.interactionManager.SelectExit(step.socket, selectedInteractable);
        }

        yield return new WaitForSeconds(0.05f);

        GameObject spawner = step.spawner;
        step.component.transform.position = spawner.transform.position;
        step.component.transform.rotation = spawner.transform.rotation;
        step.componentRb.velocity = Vector3.zero;
        step.componentRb.angularVelocity = Vector3.zero;
    }

    IEnumerator ShowNotification()
    {
        notificationPanel.SetActive(true);
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
    }

    void ShowInfoPanels(int stepIndex)
    {
        if (stepIndex > 0)
        {
            assemblySteps[stepIndex - 1].infoPanel1.SetActive(false);
            assemblySteps[stepIndex - 1].infoPanel2.SetActive(false);
        }

        assemblySteps[stepIndex].infoPanel1.SetActive(true);
        assemblySteps[stepIndex].infoPanel2.SetActive(true);
    }

    void PlaySFX(GameObject audioObject)
    {
        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
