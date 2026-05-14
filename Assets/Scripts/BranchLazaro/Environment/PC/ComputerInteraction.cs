using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    [Header("References")]
   
    public Animator armsAnimator;

    [Header("State")]
    
    private bool isSeated = true;

    public void UseComputer()
    {
        
        isSeated = !isSeated;

        if (armsAnimator != null)
        {
           
            armsAnimator.SetBool("EnPC", isSeated);
        }

        Debug.Log("Is Player Seated?: " + isSeated);
    }
}