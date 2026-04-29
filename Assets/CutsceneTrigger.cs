using UnityEngine;
using UnityEngine.Playables; // Timeline'ý çalýþtýrmak için þart

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneDirector; // Çalýþacak olan Timeline
    private bool hasTriggered = false; // Cutscene sadece 1 kere mi girsin?

    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer giren þey karakterse ve daha önce bu cutscene çalýþmadýysa
        if (other.CompareTag("Player") && !hasTriggered)
        {
            StartCutscene();
        }
    }

    void StartCutscene()
    {
        hasTriggered = true; // Tekrar tetiklenmesini engelle

        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play(); // Timeline'ý baþlat!
        }

        // Eðer karakterin yürümeye devam etmesini istemiyorsan:
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm != null) pm.canControl = false;
    }
}