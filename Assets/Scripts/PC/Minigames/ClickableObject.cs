using System;
using UnityEngine;
using UnityEngine.UI;

// Componente para el prefab del objeto clickeable.
// Un mismo prefab sirve tanto para "correcto" como "incorrecto":
// solo cambia el sprite/color y el flag IsCorrect
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class ClickableObject : MonoBehaviour
{
    [Header("Visual (opcional)")]
    [SerializeField] private Image image;
    [SerializeField] private Sprite correctSprite;
    [SerializeField] private Sprite incorrectSprite;

    public bool IsCorrect { get; private set; }
    public RectTransform RectTransform { get; private set; }

    // Se dispara una sola vez por click, pasando esta instancia.
    public event Action<ClickableObject> OnClicked;

    private Button button;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClicked?.Invoke(this));
    }

    //Prepara el objeto para una ronda y lo activa.
    public void Configurate(bool esCorrecto)
    {
        IsCorrect = esCorrecto;

        if (image != null)
            image.sprite = esCorrecto ? correctSprite : incorrectSprite;

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 anchoredPosition)
    {
        RectTransform.anchoredPosition = anchoredPosition;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
