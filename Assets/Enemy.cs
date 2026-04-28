using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Animator animator;

    [Header("Geri Savrulma (Knockback)")]
    public float knockbackForceX = 3f;  // Geriye itilme şiddeti
    public float knockbackForceY = 2f;  // Havaya sıçrama şiddeti
    public float knockbackDuration = 0.2f; // Sersemleme süresi

    private Rigidbody2D rb;
    private EnemyPatrol patrolScript; // Devriye kodunu kontrol etmek için

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        patrolScript = GetComponent<EnemyPatrol>(); // Devriye scriptini bul
    }

    // YENİ: Artık sadece hasarı değil, bize vuranın konumunu da istiyoruz (playerTransform)
    public void TakeDamage(int damage, Transform playerTransform)
    {
        currentHealth -= damage;

        if (animator != null) animator.SetTrigger("Hurt");

        if (currentHealth > 0)
        {
            // Eğer ölmediyse geriye savrulma efektini başlat
            StartCoroutine(KnockbackRoutine(playerTransform));
        }
        else
        {
            Die();
        }
    }

    IEnumerator KnockbackRoutine(Transform playerTransform)
    {
        // 1. Düşmanın kendi yürümesini (Devriyeyi) durdur ki savrulmaya karşı koymasın
        if (patrolScript != null) patrolScript.enabled = false;

        // 2. Oyuncu bizim sağımızda mı solumuzda mı?
        int direction = transform.position.x < playerTransform.position.x ? -1 : 1;

        // 3. Mevcut hızı sıfırla ve ters yöne şiddetle fırlat
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);

        // 4. Savrulma süresi kadar bekle
        yield return new WaitForSeconds(knockbackDuration);

        // 5. Süre bitince ve eğer canı hala varsa devriyeye (yürümeye) devam et
        if (currentHealth > 0 && patrolScript != null)
        {
            patrolScript.enabled = true;
        }
    }

    void Die()
    {
        if (animator != null) animator.SetTrigger("Death");

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // --- GÜNCELLENEN KISIM ---
        // GetComponentsInChildren ile hem ana nesnedeki fiziksel bedeni, 
        // hem de alt nesnedeki DamageHitbox'ı bulup hepsini tek seferde kapatıyoruz.
        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in allColliders)
        {
            col.enabled = false;
        }

        if (patrolScript != null) patrolScript.enabled = false;
        this.enabled = false;

        Destroy(gameObject, 1f);
    }
}