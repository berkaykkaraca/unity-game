using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float walkSpeed = 2f;
    private bool movingRight = true;

    [Header("Zemin ve Duvar Kontrolü")]
    public Transform edgeCheck;  // Uçurum kontrolü (Aşağıya bakar)
    public Transform wallCheck;  // YENİ: Duvar kontrolü (İleriye bakar)
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float currentSpeed = movingRight ? walkSpeed : -walkSpeed;
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        // 1. Uçurum var mı? (Sensörün olduğu yerde zemin YOKSA true olur)
        bool isAtEdge = !Physics2D.OverlapCircle(edgeCheck.position, checkRadius, groundLayer);

        // 2. Duvar var mı? (Sensörün olduğu yerde zemin VARSA true olur)
        bool isHittingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        // Eğer uçuruma geldiyse VEYA duvara çarptıysa arkasını dön
        if (isAtEdge || isHittingWall)
        {
            Flip();
        }

        GetComponentInChildren<Animator>().SetFloat("Speed", Mathf.Abs(walkSpeed));
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}