using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Edilecek Hedef")]
    public Transform target; // Buraya karakteri sürükleyeceğiz

    [Header("Kamera Ayarları")]
    public float smoothSpeed = 0.125f; // Yumuşaklık (Değer küçüldükçe daha yavaş takip eder)

    // Kameranın karakterden ne kadar uzakta duracağı
    // Z ekseninin -10 olması 2D oyunlar için zorunludur!
    public Vector3 offset = new Vector3(0f, 1f, -10f);

    void LateUpdate()
    {
        // Hedef (Karakter) sahnedeyse çalış
        if (target != null)
        {
            // Kameranın gitmek istediği asıl konum (Karakterin konumu + bizim ofsetimiz)
            Vector3 desiredPosition = target.position + offset;

            // Vector3.Lerp, A noktasından B noktasına anında değil, yumuşak bir geçiş sağlar
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kameranın pozisyonunu güncelle
            transform.position = smoothedPosition;
        }
    }
}