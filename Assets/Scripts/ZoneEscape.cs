using UnityEngine;

public class ZoneEscape : MonoBehaviour
{
    private GameManagerMarian managerMarian;

    private void Awake()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        managerMarian.TryToEscape();
        Debug.Log("Player intenta escapar");
    }
}
