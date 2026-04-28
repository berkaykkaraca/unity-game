using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer çarpan þey karakterimizse
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // Karakteri anýnda öldürmek için canýndan fazla hasar veriyoruz
                health.TakeDamage(1000);
            }
        }
    }
}