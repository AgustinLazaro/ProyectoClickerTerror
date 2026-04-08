using UnityEngine;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    [Header("Sistema de Errores")]
    public GameObject panelPopUp;
    public Button botonCerrarPopUp;
    public bool bloqueadoPorError = false;

    void Start()
    {
        
        botonCerrarPopUp.onClick.AddListener(CerrarError);

        
        panelPopUp.SetActive(false);
    }

    public void ActivarError()
    {
        bloqueadoPorError = true;
        panelPopUp.SetActive(true);
    }

    public void CerrarError()
    {
        bloqueadoPorError = false;
        panelPopUp.SetActive(false);
    }
}