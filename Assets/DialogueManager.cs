using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueLine
{
    public bool isPlayerTalking;
    [TextArea] public string sentence;
}

public class DialogueManager : MonoBehaviour
{
    public GameObject playerBalloon;
    public TextMeshProUGUI playerText;
    public GameObject npcBalloon;
    public TextMeshProUGUI npcText;

    public DialogueLine[] lines;
    public float typingSpeed = 0.05f;

    private int currentLine = 0;
    private int stopAtLine = 0; // Bu cümleye gelince dur ve Timeline'ı devam ettir
    private bool isTyping = false;
    private PlayableDirector director;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        director = FindObjectOfType<PlayableDirector>();
    }

    // Timeline'dan çağıracağımız sihirli fonksiyon
    // Örn: StartDialoguePart(0, 2) -> 0, 1 ve 2. cümleleri oynatır ve durur.
    public void StartDialoguePart(int start, int end)
    {
        if (playerMovement != null) playerMovement.canControl = false;
        if (director != null) director.Pause();

        currentLine = start;
        stopAtLine = end;
        ShowNextLine();
    }

    void Update()
    {
        if ((playerBalloon.activeSelf || npcBalloon.activeSelf) && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                if (lines[currentLine].isPlayerTalking) playerText.text = lines[currentLine].sentence;
                else npcText.text = lines[currentLine].sentence;
                isTyping = false;
            }
            else
            {
                if (currentLine < stopAtLine)
                {
                    currentLine++;
                    ShowNextLine();
                }
                else
                {
                    EndCurrentPart();
                }
            }
        }
    }

    void ShowNextLine()
    {
        bool isPlayer = lines[currentLine].isPlayerTalking;
        playerBalloon.SetActive(isPlayer);
        npcBalloon.SetActive(!isPlayer);
        StartCoroutine(TypeSentence(lines[currentLine].sentence, isPlayer ? playerText : npcText));
    }

    void EndCurrentPart()
    {
        playerBalloon.SetActive(false);
        npcBalloon.SetActive(false);
        if (director != null) director.Play();
    }

    // Finalde kontrolü geri vermek için bunu Timeline'ın en sonuna koyacağız
    public void GiveBackControl()
    {
        if (playerMovement != null) playerMovement.canControl = true;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI targetText)
    {
        isTyping = true;
        targetText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void PlayFirstPart()
    {
        StartDialoguePart(0, 2); // 0, 1 ve 2. cümleleri oynat
    }

    public void PlaySecondPart()
    {
        StartDialoguePart(3, 6); // 3, 4, 5 ve 6. cümleleri oynat (NPC geldikten sonra)
    }
}