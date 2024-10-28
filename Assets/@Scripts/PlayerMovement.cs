using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private float spd = 5;

    private float hMove;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1, 0.5f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Color32 gizmosColor;

    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float maxFallSpd = 18;
    [SerializeField] private float fallSpdMultiplier = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2 (hMove * spd, rb.velocity.y);
        Gravity();
    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpdMultiplier;
            rb.velocity = new Vector2 (rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpd, Mathf.Infinity));
        }
        else
            rb.gravityScale = baseGravity;
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer))
            return true;
        return false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        hMove = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.performed)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else if (context.canceled)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
