using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 20;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // İŞTE ÇÖZÜM BURADA: damage değerinin yanına transform'u da ekledik!
                // Böylece karakter kendisine neyin çarptığını bilecek ve tersi yöne uçacak.
                playerHealth.TakeDamage(damage, transform);
            }
        }
    }
}