using UnityEngine;

public class Chair : InteractableBase
{
    [Header("Chair Settings")]
    public Transform sitPosition; 

    [Header("Camera Limits (When Sitting)")]
    public float minYaw = -90f; 
    public float maxYaw = 0f;  

    public override void OnPressE(PlayerInteraction player)
    {
        if (!player.isSitting)
        {
            player.SitInChair(this);
        }
        else
        {
            player.StandUp();
        }
    }
}