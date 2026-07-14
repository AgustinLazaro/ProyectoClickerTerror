using UnityEngine;

public class CheatManagerMarian : MonoBehaviour
{
    [Header("Teclas")]
    [SerializeField] private KeyCode keyWin = KeyCode.F1;
    [SerializeField] private KeyCode keyLose = KeyCode.F2;
    [SerializeField] private KeyCode keyGodMode = KeyCode.F3;

    [Header("God Mode Settings")]
    [SerializeField] private GameManagerMarian gameManager;
    [SerializeField] private PlayerParanoia paranoiaManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AppController appController;

    private bool godModeActivo = false;

    private void Update()
    {
        if (Input.GetKeyDown(keyWin))
            ActivarWin();

        if (Input.GetKeyDown(keyLose))
            ActivarLose();

        if (Input.GetKeyDown(keyGodMode))
            ToggleGodMode();

        if (godModeActivo)
            MantenerGodMode();
    }

    private void ActivarWin()
    {
        Debug.Log("CHEAT: Win activado");
        gameManager.Victory();
    }

    private void ActivarLose()
    {
        Debug.Log("CHEAT: Lose activado");
        gameManager.GameOver();
    }

    private void ToggleGodMode()
    {
        godModeActivo = !godModeActivo;
        Debug.Log($"CHEAT: God Mode {(godModeActivo ? "ON" : "OFF")}");
    }

    private void MantenerGodMode()
    {
        // stamina siempre al máximo
        if (paranoiaManager != null)
            paranoiaManager.IsGodModeActivated = true;

        // puntaje al máximo instantáneo
        if (scoreManager != null)
            scoreManager.CheatMaxScore();

        // cooldowns desactivados
        if (appController != null)
            appController.CheatDeactivateCooldowns();
    }
}
