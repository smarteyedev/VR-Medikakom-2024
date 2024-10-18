using UnityEngine;
using TMPro;

public class BubbleChatAnimator : MonoBehaviour
{
    public float popScale = 1.2f;
    public float popDuration = 0.5f;
    public float stayDuration = 5.0f;
    public float smoothTransitionDuration = 0.3f;
    public float fadeDuration = 0.5f;

    public TextMeshProUGUI bubbleText;
    public string[] chatMessages;
    private int currentMessageIndex = 0;
    private bool isFinished = false;
    private bool isPaused = false;
    private bool isDefaultChatShown = false;

    public FloatingRobot robot;
    public GameObject robotCanvas;

    public int walkIndex = 2; // Index untuk menggerakkan robot
    public int lastMessageIndex;

    private RectTransform rectTransform;

    private bool meatCuttingFinished = false; // Flag untuk cek jika pemotongan selesai

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (lastMessageIndex >= chatMessages.Length)
        {
            lastMessageIndex = chatMessages.Length - 1;
        }

        if (chatMessages.Length > 0)
        {
            ShowDefaultBubbleChat();
        }

        // Subscribe ke event pemotongan daging
        PemotonganDaging.potonganDagingEvent += OnMeatCuttingFinished;
    }

    private void OnDestroy()
    {
        // Unsubscribe dari event pemotongan daging
        PemotonganDaging.potonganDagingEvent -= OnMeatCuttingFinished;
    }

    // Function untuk menghentikan bubble chat saat daging dipotong
    void OnMeatCuttingFinished(int potonganIndex)
    {
        meatCuttingFinished = true;
        HideBubbleChat();
    }

    void ShowDefaultBubbleChat()
    {
        if (isDefaultChatShown) return;

        bubbleText.text = chatMessages[currentMessageIndex];
        AnimateBubbleChat();
        isDefaultChatShown = true;
    }

    void AnimateBubbleChat()
    {
        if (isFinished || isPaused || meatCuttingFinished) return; // Tambahkan pengecekan meatCuttingFinished

        rectTransform.localScale = Vector3.zero;

        LeanTween.scale(rectTransform, Vector3.one * popScale, popDuration)
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, Vector3.one, smoothTransitionDuration)
                    .setEaseOutQuad()
                    .setDelay(stayDuration)
                    .setOnComplete(ChangeBubbleMessage);
            });
    }

    void ChangeBubbleMessage()
    {
        if (isPaused || isFinished || meatCuttingFinished) return; // Tambahkan pengecekan meatCuttingFinished

        currentMessageIndex++;

        if (currentMessageIndex > lastMessageIndex)
        {
            isFinished = true;
            return;
        }

        if (currentMessageIndex == walkIndex)
        {
            isPaused = true;

            LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, fadeDuration).setOnComplete(() =>
            {
                robot.WalkToTarget(() =>
                {
                    bubbleText.text = chatMessages[currentMessageIndex];
                    rectTransform.localScale = Vector3.zero;

                    LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, fadeDuration).setOnComplete(() =>
                    {
                        AnimateBubbleChat();
                    });

                    isPaused = false;
                });
            });
        }
        else
        {
            bubbleText.text = chatMessages[currentMessageIndex];
            rectTransform.localScale = Vector3.zero;
            AnimateBubbleChat();
        }
    }

    // Fungsi untuk menyembunyikan bubble chat
    void HideBubbleChat()
    {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, fadeDuration).setOnComplete(() =>
        {
            rectTransform.localScale = Vector3.zero; // Menghilangkan bubble chat
        });
    }
}
