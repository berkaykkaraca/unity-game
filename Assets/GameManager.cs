using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yükleme için þart

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true); // Ekraný aç
        Time.timeScale = 0f; // Oyunu dondur
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Zamaný geri akýt
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Sahneyi baþtan yükle
    }
}