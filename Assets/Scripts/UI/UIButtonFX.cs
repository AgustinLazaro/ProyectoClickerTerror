using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   

    [Header("Buttons")]
    [SerializeField] private Button button;

    [Header("Animation")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private AnimationCurve animationCurve;

   
    private IEnumerator expanding;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        
        Debug.Log("OnButtonClicked", gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter", gameObject);
       

        if (expanding != null)   
            StopCoroutine(expanding);

        expanding = Expanding();
        StartCoroutine(expanding);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit", gameObject);

        if (expanding != null)  
            StopCoroutine(expanding);

        expanding = Collapsing();
        StartCoroutine(expanding);
    }

    private IEnumerator Expanding()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.one * hoverScale;

        float onTime = 0f;
        float remaining = (targetScale.x - initialScale.x) / (targetScale.x - Vector3.one.x);
        float animDuration = remaining * this.animDuration;

        while (onTime < animDuration)
        {

            onTime += Time.unscaledDeltaTime;
            float lerp = onTime / animDuration;
            transform.localScale = initialScale + (targetScale - initialScale) * animationCurve.Evaluate(lerp);

            yield return null;
        }

        Debug.Log("End: Expanding");
        transform.localScale = targetScale;
        expanding = null;
    }

    private IEnumerator Collapsing()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.one;

        float onTime = 0f;
        float remaining = initialScale.x - targetScale.x;
        float animDuration = remaining * this.animDuration;

        while (onTime < animDuration)
        {
            onTime += Time.unscaledDeltaTime;
            float lerp = onTime / animDuration;
            transform.localScale = initialScale - (initialScale - targetScale) * lerp;

            yield return null;
        }

        Debug.Log("End: Collapsing");
        transform.localScale = targetScale;
        expanding = null;
    }
}
