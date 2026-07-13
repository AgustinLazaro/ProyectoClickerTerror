using UnityEngine;

public class Door : InteractableBase
{
    [Header("Door Settings")]
    public bool open = false;
    public float smooth = 1.0f;
    [SerializeField] private float doorOpenAngle = -90.0f;
    [SerializeField] private float doorCloseAngle = 0.0f;

    [Header("Audio")]
    public SFXEventChannelSO sfxChannel;

    void Update()
    {
        Quaternion targetRotation;

        if (open)
        {
            targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5 * smooth);
    }

    public void OpenDoor()
    {
        open = !open;
        if (sfxChannel != null)
        {
            sfxChannel.Raise(open ? SoundID.OpenDoor : SoundID.CloseDoor);
        }
    }

    public override void OnPressE(PlayerInteraction player)
    {
        OpenDoor();
    }
}