using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movimento")]
    [SerializeField] private float speed = 5;
    private float horizontal;

    [Header("Pulo")]
    [SerializeField] private float jumpPower = 16f;
    private bool isJumping = false;
    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float maxFallSpd = 18;
    [SerializeField] private float fallSpdMultiplier = 2;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1, 0.5f);
    [SerializeField] private Color32 gizmosColor;

    [Header("Dash")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 0.5f;
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;

    //[Header("Animação")]
    private bool isFacingRight = true;
    
    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Gravity();
    }

    //Inputs
    //Movimento
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
    //Pulo
    public void Jump(InputAction.CallbackContext context)
    {
        //O botão de pulo foi apertado
        if (context.started && isGrounded() && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        //O botão de pulo foi solto
        if (context.canceled)
        {
            if (rb.velocity.y > 0 && isJumping)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            isJumping = false;
        }

    }
    //Dash
    public void Dash(InputAction.CallbackContext context)
    {
        if (isDashing)
            return;
        if (canDash)
            StartCoroutine(DashLogic());
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.2f, groundLayer);
    }

    //Modificador de gravidade
    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpdMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpd, Mathf.Infinity));
        }
        else
            rb.gravityScale = baseGravity;
    }
    //Lógica do Dash
    IEnumerator DashLogic()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    //Script para virar personagem
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //Desenha no Gizmos
    private void OnDrawGizmos()
    {
        //Desenha a caixa de colisão do chão
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
