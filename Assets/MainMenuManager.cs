using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // DİKKAT: Parantez içine ana oyun sahnene verdiğin ismi BİREBİR aynı yazmalısın!
        // Resimden gördüğüm kadarıyla ana sahnenin adı "SampleScene"
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkıldı!");
        Application.Quit();
    }
}