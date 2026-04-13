using UnityEngine;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    [Header("Error System")]
    public GameObject popUpPanel;
    public Button popUpCloseButton;
    public bool errorBlocked = false;

    void Start()
    {
        
        popUpCloseButton.onClick.AddListener(CerrarError);

        
        popUpPanel.SetActive(false);
    }

    public void ActivarError()
    {
        errorBlocked = true;
        popUpPanel.SetActive(true);
    }

    public void CerrarError()
    {
        errorBlocked = false;
        popUpPanel.SetActive(false);
    }
}