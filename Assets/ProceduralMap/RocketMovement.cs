using System.Collections;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Roketin hareket hızı
    [SerializeField]
    private float minMoveDuration = 5f; // Minimum hareket süresi
    [SerializeField]
    private float maxMoveDuration = 10f; // Maksimum hareket süresi

    private Rigidbody2D rb;
    private bool movingUp = true; // Hareket yönü (true: yukarı, false: aşağı)
    private float currentMoveDuration; // Mevcut hareket süresi

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D bileşeni eksik!");
            return;
        }

        // İlk rastgele süreyi belirle
        currentMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);

        // Hareket döngüsünü başlat
        StartCoroutine(MovementLoop());
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            // Hareket yönünü belirle
            float direction = movingUp ? 1f : -1f;

            // Belirtilen süre boyunca hareket et
            float elapsedTime = 0f;
            while (elapsedTime < currentMoveDuration)
            {
                rb.velocity = new Vector2(0, direction * speed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Hareket durdur
            rb.velocity = Vector2.zero;

            // Yönü tersine çevir
            movingUp = !movingUp;

            // Yeni rastgele süreyi belirle
            currentMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);

            // Sonraki harekete geçmeden önce bir süre bekle (isteğe bağlı)
            yield return null;
        }
    }
}
