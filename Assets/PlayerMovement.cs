using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canControl = true;
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private float moveInput;

    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Görsel ve Animasyon")]
    public Animator animator;

    [Header("Dönüşüm Sistemi")]
    public GameObject normalVisual;
    public GameObject knightVisual;
    public bool isKnight = false;

    [Header("Saldırı Ayarları")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. KONTROL KİLİDİ: Kontrol yoksa girdileri sıfırla ve dur
        if (!canControl)
        {
            moveInput = 0; // Tuş girişini zorla sıfırla
            if (animator != null) animator.SetFloat("Speed", 0); // Animasyonu durdur
            return;
        }

        // --- Girişleri Oku ---
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- Zıplama ---
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // --- Dönüşüm ve Saldırı ---
        if (Input.GetKeyDown(KeyCode.T) && !isKnight)
        {
            TransformIntoKnight();
        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && isKnight)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // 2. FİZİK GÜNCELLEMESİ: Hareket ve Flipping burada olmalı
    void FixedUpdate()
    {
        // Kontrol yoksa hızı hemen kes (Timeline'ın transform hareketini bozmaz)
        if (!canControl)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // Normal hareket hızı
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Animasyon hızı
        if (animator != null) animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // --- Karakteri Döndürme (Flipping) ---
        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        if (animator != null) animator.SetTrigger("Attack");
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage, transform);
            }
        }
    }

    public void TransformIntoKnight()
    {
        isKnight = true;
        normalVisual.SetActive(false);
        knightVisual.SetActive(true);
        animator = knightVisual.GetComponent<Animator>();
        Debug.Log("Şövalyeye Dönüşüldü!");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}