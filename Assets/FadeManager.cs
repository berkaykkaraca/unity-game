using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeGroup; // FadePanel'deki CanvasGroup'u buraya sürükle
    public float fadeDuration = 1.5f; // Kararma hýzý
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Bu fonksiyonu Timeline'dan veya DialogueManager'dan çaðýracaðýz
    public void StartTransformSequence()
    {
        StartCoroutine(TransformRoutine());
    }

    IEnumerator TransformRoutine()
    {
        // 1. Ekraný Yavaþça Karart
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeGroup.alpha = 1;

        // 2. TAM O AN: Þövalyeye Dönüþ (Karakter kodundaki fonksiyonu çaðýr)
        if (playerMovement != null)
        {
            playerMovement.TransformIntoKnight();
        }

        // Kýsa bir süre karanlýkta bekle (Vurgu için)
        yield return new WaitForSeconds(0.5f);

        // 3. Ekraný Yavaþça Geri Aç
        timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeGroup.alpha = 0;

        // 4. Sonunda kontrolü geri ver
        playerMovement.canControl = true;
    }
}