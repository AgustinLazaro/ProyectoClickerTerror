using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ComputerOS : MonoBehaviour
{
    [Header("Ventanas (Paneles)")]
    public GameObject windowGames;
    public GameObject windowShop;
    public GameObject windowGameplayArea;
    public GameObject windowError;

    [Header("Botones del Escritorio")]
    public Button btnOpenGames;
    public Button btnOpenShop;
    public Button btnLaunchEndless;

    [Header("Botones de Cierre (Las X)")]
    public Button btnCloseGames;
    public Button btnCloseShop;
    public Button btnCloseGameplay;
    public Button btnCloseError;

    [Header("Sistema de Economía")]
    public TextMeshProUGUI moneyText;
    public int playerMoney = 100;

    [Header("Configuración de Errores Aleatorios")]
    [Range(0, 100)] public int chanceOfError = 20; 
    public float checkFrequency = 10f;            
    public bool errorBlocked = false;             

    void Start()
    {
        
        errorBlocked = false;
        CloseAllWindows();
        UpdateMoneyUI();

        
        btnOpenGames.onClick.AddListener(() => OpenWindow(windowGames));
        btnOpenShop.onClick.AddListener(() => OpenWindow(windowShop));
        btnLaunchEndless.onClick.AddListener(StartEndlessGame);

       
        btnCloseGames.onClick.AddListener(CloseAllWindows);
        btnCloseShop.onClick.AddListener(CloseAllWindows);
        btnCloseGameplay.onClick.AddListener(CloseAllWindows);
        btnCloseError.onClick.AddListener(CerrarError);

   
        StartCoroutine(RandomErrorRoutine());
    }



    public void OpenWindow(GameObject window)
    {
        
        if (errorBlocked)
        {
            Debug.LogWarning("Sistema bloqueado. Debes cerrar el error primero.");
            return;
        }

        CloseAllWindows();
        window.SetActive(true);
    }

    public void CloseAllWindows()
    {
        windowGames.SetActive(false);
        windowShop.SetActive(false);
        windowGameplayArea.SetActive(false);
       
    }

    void StartEndlessGame()
    {
        if (!errorBlocked)
        {
            CloseAllWindows();
            windowGameplayArea.SetActive(true);
        }
    }

 

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Saldo: $" + playerMoney;
    }

   

    IEnumerator RandomErrorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkFrequency);

            if (!errorBlocked)
            {
                int roll = Random.Range(0, 101);
                if (roll < chanceOfError)
                {
                    ActivarError();
                }
            }
        }
    }

    public void ActivarError()
    {
        errorBlocked = true;
        windowError.SetActive(true);
      
        Debug.Log("<color=red>ERROR CRÍTICO:</color> Sistema bloqueado.");
    }

    public void CerrarError()
    {
        errorBlocked = false;
        windowError.SetActive(false);
        Debug.Log("<color=green>SISTEMA RESTAURADO</color>");
    }
}