using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private LayerMask PlayerLayer;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField]private CapsuleCollider2D col;
    [SerializeField]private Vector2 frameVelocity;
    [SerializeField] Animator animator;
    
    [Header("Movement")]
    [SerializeField] private float speed = 5;
    private float horizontal;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 13;
    [SerializeField] private float slowMultiplier = 0.5f;

    private bool canJump = false;

    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float maxFallSpd = 18;
    [SerializeField] private float fallSpdMultiplier = 2;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1, 0.5f);
    [SerializeField] private Color32 gizmosColor;
    [SerializeField] private UnityAction onGround;

    [Header("Gamefeel")]
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferCounter = 0;
    [SerializeField] private float groundDistance = 5;
    private bool jumpBuffer = false;
    private InputAction.CallbackContext lastInput = new InputAction.CallbackContext();

    [Header("Dash")]
    [SerializeField] private float dashingPower = 24;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 0.5f;
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;

    //private int test = 0;

    //[Header("Animação")]
    private bool isFacingRight = true;
    
    private void Update()
    {
        CoyoteTime();
        if (CheckGroundDistance() < groundDistance)
        {
            JumpBuffer();
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
            return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        Gravity();
        EdgeDetection();
    }

    //Inputs

    //Movimento
    public void Move(InputAction.CallbackContext context)
    {
        Flip();
        horizontal = context.ReadValue<Vector2>().x;
    }
    //Pulo
    public void Jump(InputAction.CallbackContext context)
    {
        //O botão de pulo foi apertado
        //Debug.Log("Apertado");
        if (context.performed)
        {
            if (canJump)
                PerformJump(false);
            else
                jumpBuffer = true;
        }
        if (context.canceled)
        {
            //Debug.Log("Canceled");
            if (rb.velocity.y > 0)
                PerformJump(true);
            lastInput = context;
        }
        coyoteTimeCounter = 0;
    }
    //Dash
    public void Dash(InputAction.CallbackContext context)
    {
        if (isDashing)
            return;
        if (canDash)
            StartCoroutine(DashLogic());
    }

    //Funções

    //Pulo
    private void PerformJump(bool slowActive)
    {
        //float jumpStrength = Mathf.Lerp(jumpPower * 0.5f, jumpPower, jumpPressTime / maxJumpTime);

        if (slowActive)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slowMultiplier);
        else
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    //Verifica se o player está no chão
    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.2f, groundLayer))
        {

            return true;
        }
        else
        {

            return false;
        }
    }
    //Lógica do Dash
    IEnumerator DashLogic()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
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

    //Script para virar personagem
    private void Flip()
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
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

    //Gamefeel

    //Chance de pular mesmo após sair do chão
    private void CoyoteTime()
    {
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            canJump = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                canJump = true;
            }
            else
            {
                coyoteTimeCounter = 0;
                canJump = false;
            }
        }
    }
    //Pular antecipadamente
    public void JumpBuffer()
    {
        if (isGrounded())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
                if (jumpBuffer)
                {
                    jumpBufferCounter = jumpBufferTime;
                    jumpBuffer = false;
                    StartCoroutine(WaitForGround());
                }
            }
            else
            {
                jumpBufferCounter = 0;
            }
        }
    }

    //Core do JumpBuffer
    IEnumerator WaitForGround()
    {
        yield return new WaitUntil(() => isGrounded() == true);
        PerformJump(false);
    }

    //Verifica a distancia do chão
    private float CheckGroundDistance()
    {
        // Lança um Raycast 2D para baixo a partir da posição do personagem
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpPower, groundLayer);

        if (hit.collider != null)
        {
            float distanceToGround = hit.distance;
            //Debug.Log("Distância até o chão: " + distanceToGround);
        }

        // Desenhar o Raycast para depuração (opcional)
        Debug.DrawRay(transform.position, Vector2.down * hit.distance, Color.red);
        return hit.distance;
    }

    //Detecta se o player está em uma borda
    private void EdgeDetection()
    {
        bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.up, 0.5f, ~PlayerLayer);

        // Hit a Ceiling
        if (ceilingHit) frameVelocity.y = Mathf.Min(0, frameVelocity.y);
    }
}
