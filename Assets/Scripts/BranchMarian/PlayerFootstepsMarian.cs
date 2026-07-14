using UnityEngine;

public class PlayerFootstepsMarian : MonoBehaviour
{
    [Header("Configuración Pasos")]
    public float timeBetweenSteps = 0.5f;

    [Header("Conexión con Event channel SFX")]
    public SFXEventChannelSO sfxChannel;

    private PlayerMovementMarian playerMovement;
    private float timerStep;
    private bool tapStep1 = true;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovementMarian>();
    }

    void Update()
    {

        if (playerMovement != null && playerMovement.IsMoving)
        {
            timerStep -= Time.deltaTime;

            if (timerStep <= 0f)
            {

                SoundID sonidoActual = tapStep1 ? SoundID.Footstep1 : SoundID.Footstep2;
                sfxChannel.Raise(sonidoActual);

                tapStep1 = !tapStep1;
                timerStep = timeBetweenSteps;
            }
        }
        else
        {
            timerStep = 0f;
        }
    }
}
