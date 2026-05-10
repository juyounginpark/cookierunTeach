using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Cookie : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground")]
    [SerializeField] private float groundY = 0f;

    private Rigidbody2D rb;
    private float verticalVelocity;
    private int jumpsRemaining;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            verticalVelocity = jumpVelocity;
            jumpsRemaining--;
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        Vector2 pos = rb.position;
        pos.y += verticalVelocity * Time.fixedDeltaTime;

        if (pos.y <= groundY)
        {
            pos.y = groundY;
            verticalVelocity = 0f;
            isGrounded = true;
            jumpsRemaining = maxJumps;
        }
        else
        {
            isGrounded = false;
        }

        rb.MovePosition(pos);
    }
}
