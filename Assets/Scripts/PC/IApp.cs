//using TMPro;
//using UnityEngine;

using UnityEngine;

public interface IApp
{
    void OnAppOpen();
    void OnAppClose();
}


//public class PCInteraction : MonoBehaviour
//{
//    [Header("Configuracion")]
//    [SerializeField] private float velocidadZoom = 2f;

//    [Header("Referencias")]
//    [SerializeField] private Transform camaraJugador;
//    [SerializeField] private Transform puntoZoom;
//    [SerializeField] private GameObject pcCanvas;
//    [SerializeField] private PlayerMovement playerMovement;
//    [SerializeField] private CameraLook cameraLook;

//    private Vector3 posicionOriginalCamara;
//    private Quaternion rotacionOriginalCamara;
//    private bool pcActiva = false;
//    private bool enTransicion = false;

//    public bool PcActiva => pcActiva;

//    private void Update()
//    {
//        if (enTransicion) return;
//        if (pcActiva && Input.GetKeyDown(KeyCode.Escape))
//            DesactivarPC();
//    }

//    public void ActivarPC()
//    {
//        if (pcActiva || enTransicion) return;

//        posicionOriginalCamara = camaraJugador.position;
//        rotacionOriginalCamara = camaraJugador.rotation;

//        playerMovement.isSitting = true;
//        cameraLook.enabled = false;

//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;

//        StartCoroutine(ZoomHaciaMonitor());
//    }

//    public void DesactivarPC()
//    {
//        if (!pcActiva || enTransicion) return;

//        pcCanvas.SetActive(false);
//        StartCoroutine(ZoomDesdeMonitor());
//    }

//    private IEnumerator ZoomHaciaMonitor()
//    {
//        enTransicion = true;

//        while (Vector3.Distance(camaraJugador.position, puntoZoom.position) > 0.01f)
//        {
//            camaraJugador.position = Vector3.Lerp(
//                camaraJugador.position,
//                puntoZoom.position,
//                Time.deltaTime * velocidadZoom
//            );
//            camaraJugador.rotation = Quaternion.Lerp(
//                camaraJugador.rotation,
//                puntoZoom.rotation,
//                Time.deltaTime * velocidadZoom
//            );
//            yield return null;
//        }

//        camaraJugador.position = puntoZoom.position;
//        camaraJugador.rotation = puntoZoom.rotation;

//        pcCanvas.SetActive(true);
//        pcActiva = true;
//        enTransicion = false;
//    }

//    private IEnumerator ZoomDesdeMonitor()
//    {
//        enTransicion = true;
//        pcActiva = false;

//        while (Vector3.Distance(camaraJugador.position, posicionOriginalCamara) > 0.01f)
//        {
//            camaraJugador.position = Vector3.Lerp(
//                camaraJugador.position,
//                posicionOriginalCamara,
//                Time.deltaTime * velocidadZoom
//            );
//            camaraJugador.rotation = Quaternion.Lerp(
//                camaraJugador.rotation,
//                rotacionOriginalCamara,
//                Time.deltaTime * velocidadZoom
//            );
//            yield return null;
//        }

//        camaraJugador.position = posicionOriginalCamara;
//        camaraJugador.rotation = rotacionOriginalCamara;

//        playerMovement.isSitting = false;
//        cameraLook.enabled = true;

//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;

//        enTransicion = false;
//    }
//}