using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private MinigameManager minigameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        minigameManager = GameObject.FindGameObjectWithTag("Minigame Manager").GetComponent<MinigameManager>();
    }

    private void Update()
    {
        rb.linearVelocity = Vector2.left * (speed * minigameManager.speedMultiplier);
    }
}
