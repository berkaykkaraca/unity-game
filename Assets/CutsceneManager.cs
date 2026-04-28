using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    void Start()
    {
        // 1. Ajan: Acaba Start fonksiyonu hiç çalışıyor mu?
        Debug.Log("TEST 1: CutsceneManager Start() çalıştı!");

        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        // 2. Ajan: Acaba karakteri bulabildi mi?
        if (player != null)
        {
            player.canControl = false;
            Debug.Log("TEST 2: Karakter bulundu ve Şalter İndirildi!");
        }
        else
        {
            // 3. Ajan: Karakteri bulamadıysa bize haber versin
            Debug.LogWarning("DİKKAT: Sahnede PlayerMovement kodu bulunamadı!");
        }
    }
}