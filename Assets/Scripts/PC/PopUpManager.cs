using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float intervalAppearance = 8f;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private Transform popUpContainer;

    [Header("Menssages")]
    [SerializeField]
    private string[] messages =
    {
        "ERROR: archivo corrupto",
        "ADVERTENCIA: sistema inestable",
        "ERROR: conexión perdida",
        "ALERTA: proceso desconocido",
        "ERROR: memoria insuficiente"
    };

    [SerializeField] private PCAudioPlayer pcAudio;

    private float _nextTimePopUp;
    private GameObject _currentPopUp = null;

    private void Awake()
    {
        _nextTimePopUp = intervalAppearance;
    }

    private void Update()
    {
        if (_currentPopUp != null) return;    

        _nextTimePopUp -= Time.deltaTime;

        if (_nextTimePopUp <= 0f)
            ShowPopUp();
    }

    private void ShowPopUp()
    {
        pcAudio.PlaySound(SoundID.Error);
        string mensaje = messages[Random.Range(0, messages.Length)];

        _currentPopUp = Instantiate(popUpPrefab, popUpContainer);
        _currentPopUp.GetComponent<PopUp>().Inicialize(mensaje, this);

       
        RectTransform rect = _currentPopUp.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(
            Random.Range(-300f, 300f),
            Random.Range(-150f, 150f)
        );

        _nextTimePopUp = intervalAppearance;
    }

    public void ClosePopUp()
    {
        if (_currentPopUp == null) return;

        pcAudio.PlaySound(SoundID.Click);

        Destroy(_currentPopUp);
        _currentPopUp = null;
    }
}
