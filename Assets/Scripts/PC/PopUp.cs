using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI messageText;

    private PopUpManager popUpManager;

    private void Awake()
    {
        popUpManager = GetComponentInParent<PopUpManager>();
        closeButton.onClick.AddListener(Close);
    }

    public void Inicialize(string mensaje, PopUpManager manager)
    {
        popUpManager = manager;
        messageText.text = mensaje;
    }

    private void Close()
    {
        popUpManager.ClosePopUp();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveAllListeners();
    }
}
