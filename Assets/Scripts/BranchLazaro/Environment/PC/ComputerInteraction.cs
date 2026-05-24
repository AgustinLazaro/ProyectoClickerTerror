using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private PCInteraction pcInteraction;   

    [Header("State")]
    private bool isUsingPC = false; 

    private void Awake()
    {
        if (pcInteraction == null)
        {
            pcInteraction = FindAnyObjectByType<PCInteraction>();
        }
    }

    public void UseComputer()
    {
        
        isUsingPC = !isUsingPC;

        
        if (armsAnimator != null)
            armsAnimator.SetBool("EnPC", isUsingPC);

       
        if (isUsingPC)
            pcInteraction.ActivePC();  
        else
            pcInteraction.DeactivePC(); 

        Debug.Log("Is Player Using PC?: " + isUsingPC);
    }
}