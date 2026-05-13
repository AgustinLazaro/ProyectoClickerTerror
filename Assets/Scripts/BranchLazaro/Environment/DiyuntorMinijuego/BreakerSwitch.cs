using UnityEngine;

public class BreakerSwitch : MonoBehaviour
{
    public bool isOn = false;
    public BreakerManager manager;

    [Header("Visual Feedback")]
    public Material offMaterial;
    public Material onMaterial;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        transform.localEulerAngles = new Vector3(0, 0, 0);
        if (meshRenderer != null && offMaterial != null)
        {
            meshRenderer.material = offMaterial;
        }
    }

    public void Interact()
    {
        if (!isOn)
        {
            isOn = true;
            transform.localEulerAngles = new Vector3(-45, 0, 0);

            if (meshRenderer != null && onMaterial != null)
            {
                meshRenderer.material = onMaterial;
            }

            if (manager != null) manager.CheckSwitches();
        }
    }
}