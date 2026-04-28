using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Playables; // YENİ: Timeline'ı kontrol etmek için kütüphane eklendi!

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed = 0.05f;

    private bool isTyping = false;
    private PlayerMovement playerMovement;
    private PlayableDirector director; // YENİ: Yönetmenimiz (Timeline)

    void OnEnable()
    {
        // Kutucuk açıldığında oyuncuyu kilitli tut
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null) playerMovement.canControl = false;

        // DİYALOG BAŞLADI: Yönetmeni (Timeline) bul ve DONDUR!
        director = FindObjectOfType<PlayableDirector>();
        if (director != null) director.Pause();

        textDisplay.text = "";
        index = 0;
        StartCoroutine(TypeSentence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTyping)
            {
                NextSentence();
            }
            else
            {
                StopAllCoroutines();
                textDisplay.text = sentences[index];
                isTyping = false;
            }
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            textDisplay.text = "";
            gameObject.SetActive(false);

            // DİYALOG BİTTİ: Yönetmene "Devam Et" emri ver! 
            // (Artık kontrolü burada vermiyoruz, sinematik tamamen bitince vereceğiz)
            if (director != null) director.Play();
        }
    }
}