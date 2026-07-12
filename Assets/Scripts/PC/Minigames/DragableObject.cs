using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private DragObjectGame game;
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 offset; // distancia entre cursor y centro del objeto

    private void Awake()
    {
        game = GetComponentInParent<DragObjectGame>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        //calcula el offset entre el cursor y el centro del objeto
       Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
            );
        offset = rectTransform.anchoredPosition - localPoint;
        game.OnBeginDrag(rectTransform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
            );

        // aplica el offset para que el objeto no salte
        rectTransform.anchoredPosition = localPoint + offset;

        game.OnDrag(rectTransform, Vector2.zero);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        game.OnEndDrag(rectTransform);
    }
}
