using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator armsAnimator;
    [SerializeField] PCInteraction pcInteraction;   //MARIAN

    [Header("State")]
    private bool isSeated = true;

    private void Awake()
    {
        pcInteraction = FindAnyObjectByType<PCInteraction>();
    }

    public void UseComputer()
    {
        isSeated = !isSeated;

        if (armsAnimator != null)
            armsAnimator.SetBool("EnPC", isSeated);

        if (isSeated)
            pcInteraction.ActivePC();   //MARIAN line
        else
            pcInteraction.DeactivePC(); //MARIAN line

        Debug.Log("Is Player Seated?: " + isSeated);
    }
}