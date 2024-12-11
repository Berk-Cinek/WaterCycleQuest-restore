using System.Collections;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Kuşun hareket hızı
    [SerializeField]
    private float minMoveDuration = 5f; // Minimum hareket süresi
    [SerializeField]
    private float maxMoveDuration = 10f; // Maksimum hareket süresi

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool movingRight = true; // Hareket yönü (true: sağa, false: sola)
    private float currentMoveDuration; // Mevcut hareket süresi

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D bileşeni eksik!");
            return;
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer bileşeni eksik!");
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
            float direction = movingRight ? 1f : -1f;

            // Belirtilen süre boyunca hareket et
            float elapsedTime = 0f;
            while (elapsedTime < currentMoveDuration)
            {
                rb.velocity = new Vector2(direction * speed, 0); // Yatay eksende hareket
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Hareket durdur
            rb.velocity = Vector2.zero;

            // Yönü tersine çevir
            movingRight = !movingRight;

            // Flip işlemini gerçekleştir
            spriteRenderer.flipX = !spriteRenderer.flipX;

            // Yeni rastgele süreyi belirle
            currentMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);

            // Sonraki harekete geçmeden önce bir süre bekle (isteğe bağlı)
            yield return null;
        }
    }
}
