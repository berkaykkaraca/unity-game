using System.Collections; // Coroutine (Zamanlayıcı) için gerekli
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Arayüz (UI)")]
    public Image healthBarFill;

    [Header("Hasar Ayarları")]
    public float iFramesDuration = 0.5f; // Yarım saniye ölümsüzlük süresi
    private float iFramesTimer; // Sayacımız

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Eğer ölümsüzlük süremiz varsa, süreyi geriye doğru saydır (0'a kadar)
        if (iFramesTimer > 0)
        {
            iFramesTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.H)) TakeDamage(10);
    }

    public void TakeDamage(int damage, Transform enemyTransform = null)
    {
        // KORUMA 1: Eğer karakter zaten öldüyse veya ölümsüzlük süresi bitmediyse hasar alma!
        if (currentHealth <= 0 || iFramesTimer > 0) return;

        // Hasarı al ve ölümsüzlük sayacını tekrar fulle
        currentHealth -= damage;
        iFramesTimer = iFramesDuration;

        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthUI();

        // Ölmediysek ve bize vuran biri varsa geriye savrul
        if (currentHealth > 0 && enemyTransform != null)
        {
            StartCoroutine(KnockbackRoutine(enemyTransform));
        }
        else if (currentHealth == 0)
        {
            Die();
        }
    }

    // Zamanlayıcılı Savurma Fonksiyonu
    // Zamanlayıcılı Savurma Fonksiyonu
    IEnumerator KnockbackRoutine(Transform enemyTransform)
    {
        // 1. KESİN ÇÖZÜM: Yürüyüş kodunu (Component) tamamen devredışı bırak!
        if (playerMovement != null) playerMovement.enabled = false;

        // 2. Savurma Kuvvetini Uygula
        Vector2 difference = (transform.position - enemyTransform.position).normalized;

        // Kuvveti biraz artırdık (Eğer karakterin Rigidbody Mass değeri yüksekse 5 yetmez)
        Vector2 force = new Vector2(difference.x * 6f, 4f);

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        // 3. Karakter havadayken 0.3 saniye bekle
        yield return new WaitForSeconds(0.2f);

        // 4. Yürüyüş kodunu tekrar aç (Fişi tak)
        if (playerMovement != null && currentHealth > 0) playerMovement.enabled = true;
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Karakter Öldü!");
        if (playerMovement != null) playerMovement.canControl = false;
        rb.velocity = Vector2.zero; // Ölünce kaymayı durdur
    }
}