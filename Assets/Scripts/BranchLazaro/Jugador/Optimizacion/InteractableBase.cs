using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Base Settings")]
    public string interactText = "Interact";
    public bool isInteractable = true;

    [Header("Hover Visuals (Optional)")]
    public MeshRenderer targetMesh;
    public Material hoverMaterial;
    private Material defaultMaterial;

    protected virtual void Start()
    {
        if (targetMesh != null)
        {
            defaultMaterial = targetMesh.material;
        }
    }

    public virtual void OnHoverEnter()
    {
        if (targetMesh != null && hoverMaterial != null && isInteractable)
        {
            targetMesh.material = hoverMaterial;
        }
    }

    public virtual void OnHoverExit()
    {
        if (targetMesh != null && defaultMaterial != null)
        {
            targetMesh.material = defaultMaterial;
        }
    }

    public virtual void OnClickDown(PlayerInteraction player) { }
    public virtual void OnClickHold(PlayerInteraction player) { }
    public virtual void OnPressE(PlayerInteraction player) { }
}