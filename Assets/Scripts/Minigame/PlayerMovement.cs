using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float jump;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Animator animator;
    private static readonly int State = Animator.StringToHash("State");

    enum PlayerState
    {
        Walk = 0,
        Slide = 1,
    };

    [SerializeField] private PlayerState playerState = PlayerState.Walk;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetInteger(State, (int)playerState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            rb.AddForce(Vector2.up * jump);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerState = PlayerState.Slide;
            animator.SetInteger(State, (int)playerState);
            Invoke(nameof(ResetAnim), 1);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            playerState = PlayerState.Walk;
            animator.SetInteger(State, (int)playerState);
            Invoke(nameof(ResetAnim), 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    private void ResetAnim()
    {
        playerState = PlayerState.Walk;
        animator.SetInteger(State, (int)playerState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            SceneManager.LoadScene("Minigame");
    }
}
