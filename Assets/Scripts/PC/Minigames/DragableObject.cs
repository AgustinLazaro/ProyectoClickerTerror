using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private DragObjectGame game;
    private Canvas canvas;

    private void Awake()
    {
        game = GetComponentInParent<DragObjectGame>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        game.OnDrag(eventData.delta / canvas.scaleFactor);
        //game.OnDrag(eventData.delta / canvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        game.OnEndDrag();
    }
}
