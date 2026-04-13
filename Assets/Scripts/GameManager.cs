using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;
    private bool gameOver = false;

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GanarJuego()
    {
        if (gameOver) return;

        gameOver = true;
        victoryScreen.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
    }

    public void PerderJuego()
    {
        if (gameOver) return;

        gameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
