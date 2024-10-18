using UnityEngine;

public class BubbleChatAnimation : MonoBehaviour
{
    public float popScale = 1.2f;        // Skala pop
    public float popDuration = 0.5f;     // Durasi animasi pop
    public float stayDuration = 5.0f;     // Durasi stay sebelum menghilang

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AnimateBubbleChat();
    }

    private void AnimateBubbleChat()
    {
        rectTransform.localScale = Vector3.zero; // Mulai dari ukuran nol

        // Animasi scale menggunakan LeanTween
        LeanTween.scale(rectTransform, Vector3.one * popScale, popDuration)
            .setEaseOutQuad() // Efek easing
            .setOnComplete(() =>
            {
                LeanTween.scale(rectTransform, Vector3.one, popDuration) // Kembali ke ukuran normal
                    .setEaseOutQuad()
                    .setDelay(stayDuration) // Tunggu sebelum menghilang
                    .setOnComplete(DismissBubbleChat); // Panggil fungsi untuk menghilangkan chat
            });
    }

    private void DismissBubbleChat()
    {
        LeanTween.scale(rectTransform, Vector3.zero, popDuration) // Animasi menghilang
            .setEaseOutQuad()
            .setOnComplete(() => Destroy(gameObject)); // Hapus objek setelah animasi selesai
    }
}
