using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.Collections;

public class TeleportationManagerWithFade : MonoBehaviour
{
    public Transform playerRig; // XR Rig pemain
    public Transform cameraTransform; // Transform dari kamera (HMD)
    public Transform mainMenuPoint; // Posisi Main Menu
    public Transform[] teleportPoints; // Array teleport points untuk Level 1, Level 2, Level 3
    public GameObject[] teleportPanels; // Panel di setiap titik teleportasi (Level 1, 2, 3)
    public Image fadeImage; // Referensi ke komponen Image untuk fade effect
    public float fadeDuration = 1.0f; // Durasi fade out

    private bool isTeleporting = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private int targetLevelIndex;

    private void Start()
    {
        HideAllPanels();
    }

    // Fungsi untuk teleport ke level tertentu dengan fade effect
    public void TeleportToLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < teleportPoints.Length)
        {
            targetLevelIndex = levelIndex;
            StartCoroutine(FadeOutIn(true)); // True untuk teleportasi level tertentu
        }
        else
        {
            Debug.LogError("Level index out of range!");
        }
    }

    // Fungsi untuk teleport ke Main Menu dengan fade effect
    public void TeleportToMainMenu()
    {
        StartCoroutine(FadeOutIn(false)); // False untuk teleportasi ke Main Menu
    }

    private IEnumerator FadeOutIn(bool toLevel)
    {
        // Fade Out
        yield return FadeTo(1); // Fade ke alpha 1 (sepenuhnya terlihat)

        // Teleportasi setelah fade out selesai
        if (toLevel)
        {
            ExecuteTeleport(targetLevelIndex);
        }
        else
        {
            ExecuteTeleportToMainMenu();
        }

        // Tunggu selama 2 detik sebelum fade in
        yield return new WaitForSeconds(2f);

        // Fade In
        yield return FadeTo(0); // Fade kembali ke alpha 0 (transparan)
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }

    private void ExecuteTeleport(int levelIndex)
    {
        targetPosition = teleportPoints[levelIndex].position;
        targetRotation = teleportPoints[levelIndex].rotation;
        TeleportPlayerTo(targetPosition, targetRotation);
        ShowPanelAtTeleportPoint(levelIndex);
    }

    private void ExecuteTeleportToMainMenu()
    {
        targetPosition = mainMenuPoint.position;
        targetRotation = mainMenuPoint.rotation;
        TeleportPlayerTo(targetPosition, targetRotation);
        HideAllPanels();
    }

    private void TeleportPlayerTo(Vector3 targetPosition, Quaternion targetRotation)
    {
        float yOffset = 1f;
        targetPosition = new Vector3(targetPosition.x, targetPosition.y + yOffset, targetPosition.z);

        Vector3 cameraOffset = playerRig.position - cameraTransform.position;
        Vector3 adjustedPosition = targetPosition + cameraOffset;

        Rigidbody rigBody = playerRig.GetComponent<Rigidbody>();
        if (rigBody != null)
        {
            rigBody.isKinematic = true;
        }

        playerRig.position = adjustedPosition;
        playerRig.rotation = targetRotation;

        Physics.SyncTransforms();

        if (rigBody != null)
        {
            rigBody.isKinematic = false;
        }
    }

    private void ShowPanelAtTeleportPoint(int index)
    {
        HideAllPanels();
        if (index >= 0 && index < teleportPanels.Length)
        {
            teleportPanels[index].SetActive(true);
        }
    }

    private void HideAllPanels()
    {
        foreach (GameObject panel in teleportPanels)
        {
            panel.SetActive(false);
        }
    }
}
