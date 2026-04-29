using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Playables; // YENİ: Timeline'ı durdurup başlatmak için gerekli!

[System.Serializable]
public struct SpeakerUI
{
    public string characterName;
    public GameObject balloon;
    public TextMeshProUGUI textComponent;
}

[System.Serializable]
public struct DialogueLine
{
    public int speakerIndex;
    [TextArea(3, 10)]
    public string sentence;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Timeline Bağlantısı")]
    public PlayableDirector director; // Timeline'ı kontrol edeceğimiz yer

    [Header("Konuşmacılar (0: Oyuncu, 1: NPC_1...)")]
    public SpeakerUI[] speakers;

    [Header("Senaryo ve Yazı Hızı")]
    public DialogueLine[] lines;
    public float typingSpeed = 0.04f; // Harflerin geliş hızı

    private int currentLineIndex = 0;
    private int endLineIndex = 0;
    private bool isDialogueActive = false;

    private bool isTyping = false; // Şu an harfler dökülüyor mu?
    private Coroutine typingCoroutine; // Harf harf yazma işlemini tutan sayaç

    void Update()
    {
        // Diyalog aktifken boşluk veya sol tık basılırsa
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                // Eğer hala yazıyorsa, yazmayı kes ve cümleyi anında tamamla!
                StopCoroutine(typingCoroutine);
                CompleteSentence();
            }
            else
            {
                // Eğer yazı zaten bittiyse bir sonraki cümleye geç
                NextSentence();
            }
        }
    }

    public void PlayFirstPart() { StartDialoguePart(0, 2); }
    public void PlaySecondPart() { StartDialoguePart(3, 6); }

    public void PlayThirdPart() { StartDialoguePart(0, 2); }
    public void StartDialoguePart(int startIndex, int endIndex)
    {
        currentLineIndex = startIndex;
        endLineIndex = endIndex;
        isDialogueActive = true;

        // 1. DİYALOG BAŞLADI, TİMELİNE'I DONDUR! (Erken kararmayı engeller)
        if (director != null) director.Pause();

        ShowCurrentSentence();
    }

    void NextSentence()
    {
        currentLineIndex++;

        if (currentLineIndex > endLineIndex)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentSentence();
        }
    }

    void ShowCurrentSentence()
    {
        // Önce herkesin balonunu kapat
        foreach (SpeakerUI speaker in speakers)
        {
            if (speaker.balloon != null) speaker.balloon.SetActive(false);
        }

        DialogueLine currentLine = lines[currentLineIndex];
        int activeSpeakerID = currentLine.speakerIndex;

        if (activeSpeakerID >= 0 && activeSpeakerID < speakers.Length)
        {
            SpeakerUI activeSpeaker = speakers[activeSpeakerID];
            activeSpeaker.balloon.SetActive(true);

            // Daktilo efektini başlat
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(activeSpeaker.textComponent, currentLine.sentence));
        }
    }

    // Harf harf yazdıran sihirli fonksiyon
    IEnumerator TypeSentence(TextMeshProUGUI textComp, string sentence)
    {
        isTyping = true;
        textComp.text = ""; // Kutuyu temizle

        foreach (char letter in sentence.ToCharArray())
        {
            textComp.text += letter; // Harfleri tek tek ekle
            yield return new WaitForSeconds(typingSpeed); // Biraz bekle
        }

        isTyping = false; // Yazma işlemi bitti
    }

    // Space'e basılınca cümleyi anında bitiren fonksiyon
    void CompleteSentence()
    {
        isTyping = false;
        DialogueLine currentLine = lines[currentLineIndex];
        int activeSpeakerID = currentLine.speakerIndex;

        if (activeSpeakerID >= 0 && activeSpeakerID < speakers.Length)
        {
            // Cümlenin tamamını direkt ekrana bas
            speakers[activeSpeakerID].textComponent.text = currentLine.sentence;
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;

        foreach (SpeakerUI speaker in speakers)
        {
            if (speaker.balloon != null) speaker.balloon.SetActive(false);
        }

        // 2. DİYALOG BİTTİ, TİMELİNE KALDIĞI YERDEN AKMAYA DEVAM ETSİN!
        if (director != null) director.Play();
    }

    public void GiveBackControl()
    {
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm != null) pm.canControl = true;
    }
}