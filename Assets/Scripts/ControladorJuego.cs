using UnityEngine;
using UnityEngine.SceneManagement; 

public class ControladorJuego : MonoBehaviour
{
    public GameObject pantallaGameOver;
    public GameObject pantallaVictoria;
    private bool juegoTerminado = false;

    void Update()
    {
      
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GanarJuego()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        pantallaVictoria.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
    }

    public void PerderJuego()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        pantallaGameOver.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
