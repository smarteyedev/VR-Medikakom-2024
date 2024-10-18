using UnityEngine;

public class FloatingRobot : MonoBehaviour
{
    public float floatHeight = 0.5f;    
    public float floatDuration = 2f;    
    public Transform target;            
    public float walkDuration = 3f;     

    private Vector3 startPosition;
    private bool isWalking = false;
    private bool isFloating = false;

    void Start()
    {
        startPosition = transform.position;
        StartFloating();
    }

    void StartFloating()
    {
        if (isFloating) return;

        LeanTween.moveY(gameObject, startPosition.y + floatHeight, floatDuration)
            .setEaseInOutSine()
            .setLoopPingPong();   
        
        isFloating = true;
    }

    public void WalkToTarget(System.Action onComplete)
    {
        if (isWalking || target == null) return;
        isWalking = true;

        // Gerakan berjalan ke target dengan animasi naik-turun
        LeanTween.move(gameObject, target.position, walkDuration)
            .setEaseInOutSine()
            .setOnUpdate((float val) =>
            {
                // Floating while moving towards the target
                float yOffset = Mathf.Sin(Time.time * Mathf.PI * 2 / floatDuration) * floatHeight;
                transform.position = new Vector3(transform.position.x, startPosition.y + yOffset, transform.position.z);
            })
            .setOnComplete(() =>
            {
                isWalking = false;
                onComplete?.Invoke(); // Memanggil callback setelah selesai berjalan
            });
    }
}
