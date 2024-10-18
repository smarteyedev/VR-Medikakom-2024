using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleChatManager : MonoBehaviour
{
    public GameObject bubbleChat; 
    public TextMeshProUGUI bubbleText; 
    public float bubbleDuration = 5.0f; 

    [System.Serializable]
    public struct BubbleElement
    {
        public int elementIndex;
        public string message;
    }

    public BubbleElement[] bubbleElements;
    public List<string> additionalBubbleMessages = new List<string>(); // List untuk pesan tambahan

    private Coroutine currentChatCoroutine; // Menyimpan referensi coroutine saat ini

    private void OnEnable()
    {
        // Daftarkan event potongan daging
        PemotonganDaging.potonganDagingEvent += ShowMeatBubbleChat;
        // Daftarkan event pemotongan selesai
        PemotonganDaging.potonganDagingSelesaiEvent += OnCuttingComplete; // Menangani pemotongan selesai
    }

    private void OnDisable()
    {
        // Hapus event saat script dinonaktifkan
        PemotonganDaging.potonganDagingEvent -= ShowMeatBubbleChat;
        PemotonganDaging.potonganDagingSelesaiEvent -= OnCuttingComplete; // Hapus event pemotongan selesai
    }

    private void Start()
    {
        if (bubbleChat != null)
        {
            bubbleChat.SetActive(false); 
        }
    }

    // Menampilkan chat bubble saat event dipanggil
    public void ShowMeatBubbleChat(int potonganIndex)
    { 
        bool elementFound = false;

        foreach (BubbleElement element in bubbleElements)
        {
            if (element.elementIndex == potonganIndex)
            {
                elementFound = true;

                if (bubbleChat != null)
                {
                    // Hentikan coroutine sebelumnya jika ada
                    if (currentChatCoroutine != null)
                    {
                        StopCoroutine(currentChatCoroutine);
                    }

                    // Jalankan coroutine untuk menampilkan chat bubble
                    currentChatCoroutine = StartCoroutine(DisplayBubbleChat(element.message, false));
                }
                break;
            }
        }
    }

    // Callback yang menandai pemotongan sudah selesai
    private void OnCuttingComplete()
    {
        // Tampilkan pesan tambahan jika potongan terakhir
        if (additionalBubbleMessages.Count > 0)
        {
            StartCoroutine(DisplayAdditionalBubbleChat());
        }
    }

    // Gabungan method DisplayBubbleChat dengan flag untuk bubble terakhir
    private IEnumerator DisplayBubbleChat(string message, bool isLastMessage)
    {
        // Jika bubble chat sedang aktif, tunggu hingga hilang
        if (bubbleChat.activeSelf)
        {
            yield return AnimateBubbleChatOut();
        }

        // Set pesan baru dan tampilkan bubble chat
        bubbleText.text = message;
        bubbleChat.SetActive(true);
        yield return AnimateBubbleChatIn();

        // Tunggu beberapa detik sesuai dengan durasi bubble
        yield return new WaitForSeconds(bubbleDuration);

        // Hanya hilangkan bubble jika ini bukan pesan terakhir
        if (!isLastMessage)
        {
            yield return AnimateBubbleChatOut();
        }
    }

    // Coroutine untuk menampilkan pesan tambahan
    private IEnumerator DisplayAdditionalBubbleChat()
    {
        // Tampilkan pesan tambahan berdasarkan index
        for (int i = 0; i < additionalBubbleMessages.Count; i++)
        {
            // Tentukan apakah ini pesan terakhir
            bool isLastMessage = (i == additionalBubbleMessages.Count - 1);

            // Set pesan tambahan dan tampilkan bubble chat
            currentChatCoroutine = StartCoroutine(DisplayBubbleChat(additionalBubbleMessages[i], isLastMessage));

            // Tunggu hingga coroutine selesai sebelum melanjutkan ke pesan berikutnya
            yield return currentChatCoroutine;
        }

        // Log di console jika tidak ada bubble chat yang tersisa
        Debug.Log("Semua pesan tambahan telah ditampilkan.");
    }

    private IEnumerator AnimateBubbleChatOut()
    {
        RectTransform rectTransform = bubbleChat.GetComponent<RectTransform>();
        LeanTween.scale(rectTransform, Vector3.zero, 0.5f).setEaseOutQuad();
        yield return new WaitForSeconds(0.5f); // Tunggu hingga animasi selesai
        bubbleChat.SetActive(false);
    }

    private IEnumerator AnimateBubbleChatIn()
    {
        RectTransform rectTransform = bubbleChat.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        LeanTween.scale(rectTransform, Vector3.one * 1.2f, 0.5f)
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, Vector3.one, 0.5f).setEaseOutQuad();
            });
        yield return new WaitForSeconds(0.5f); // Tunggu hingga animasi selesai
    }
}
