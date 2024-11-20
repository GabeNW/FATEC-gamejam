using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#region Variables
	[Header("Dependencies")]
	[SerializeField] private LayerMask PlayerLayer;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private SpriteRenderer sprRenderer;
	[SerializeField] Animator animator;
	private InputReader inputReader;
	private PlayerEvents playerEvents;
	
	[SerializeField]private CapsuleCollider2D col;
	[SerializeField]private Vector2 frameVelocity;
	
	[Header("Movement")]
	[SerializeField] private float speed = 5;
	public bool canMove = true;
	private float horizontal;

	[Header("Jump")]
	[SerializeField] private float jumpPower = 13;
	[SerializeField] private float slowMultiplier = 0.1f;
	[SerializeField] private float airControl = 1.3f;
	private bool isJumping = false;
	private bool canJump = false;
	private bool isGrounded;
	
	[Header("Gravity")]
	[SerializeField] private float baseGravity = 1;
	[SerializeField] private float maxFallSpd = 18;
	[SerializeField] private float fallSpdMultiplier = 2;
	
	[Header("Gamefeel")]
	[Header("Coyote Time")]
	[SerializeField] private float coyoteTime = 0.2f;
	private float coyoteTimeCounter;

	[Header("Jump Buffer")]
	[SerializeField] private float jumpBufferTime = 1;
	[SerializeField] private float groundDistance = 5;
	private float jumpBufferTimeCounter = 0;
	private float pressTime = 0;
	private float canceledJumpTimer = 0;
	private bool canJumpBuffer = false;
	private bool jumpBufferActive = false;
	private bool canJumpCounter = false;
	private bool canNerfJump = false;
	private bool jumpCanceled = false;
	private bool executeDelayJump = false;

	[Header("Dash")]
	[SerializeField] private float dashingPower = 24;
	[SerializeField] private float dashingTime = 0.2f;
	[SerializeField] private float dashingCooldown = 0.5f;
	[SerializeField] private TrailRenderer tr;
	private bool canDash = true;
	private bool isDashing;

	//[Header("Animação")]
	private bool isFacingRight = true;
#endregion

#region Inputs
	//Input Reader
	public void InitializeInput(InputReader inputReader)
	{
		this.inputReader = inputReader;
	}
	
	public void InitializeEvents(PlayerEvents playerEvents)
	{
		this.playerEvents = playerEvents;
	}
	
	private void OnEnable()
	{
		//Eventos de Input
		inputReader.MoveEvent += OnMove;
		
		inputReader.JumpEvent += OnJump;
		inputReader.JumpCanceledEvent += OnJumpCanceled;
		
		inputReader.DashEvent += OnDash;
		
		//Eventos de GroundCheck (PlayerEvents)
		playerEvents.onGround += HandleGrounded;
		playerEvents.onAir += HandleAirborne;
	}

	private void OnDisable()
	{
		//Eventos de Input
		inputReader.MoveEvent -= OnMove;
		
		inputReader.JumpEvent -= OnJump;
		inputReader.JumpCanceledEvent -= OnJumpCanceled;
		
		inputReader.DashEvent -= OnDash;
		
		//Eventos de GroundCheck (PlayerEvents)
		playerEvents.onGround -= HandleGrounded;
		playerEvents.onAir -= HandleAirborne;
	}
	
	//Inputs
	//Move
	public void OnMove(Vector2 move)
	{
		Flip();
		horizontal = move.x;
	}

	//Dash
	public void OnDash()
	{
		if (canMove) 
		{	
			if (isDashing)
				return;
			if (canDash && GameManager.Instance.currentLevel.canDash)
				StartCoroutine(DashLogic());
		}
	}
	
	//Jump
	private void OnJump()
	{
		if (canMove) 
		{	
			//Jump
			if (canJump)
				PerformJump(false);
			
			//JumpBuffer
			if (canJumpBuffer && !isGrounded)
				jumpBufferActive = true;
			canJumpCounter = true;
			pressTime = 0;
			
			//CoyoteTime
			coyoteTimeCounter = 0;
		}
	}
	private void OnJumpCanceled()
	{
#if UNITY_EDITOR
		//Debug.Log("Canceled Input");
#endif
		if (rb.velocity.y > 0 && isJumping)
			PerformJump(true);
		if (jumpBufferActive && !isJumping) 
			canNerfJump = true;
		coyoteTimeCounter = 0;
	}
#endregion

#region Events
	//Events
	private void HandleGrounded()
	{
		isGrounded = true;
		animator.SetBool("IsJumping", !isGrounded);
		//CoyoteTime
		coyoteTimeCounter = coyoteTime;
		canJump = true;
		
		//JumpBuffer
		jumpBufferTimeCounter = jumpBufferTime;
		if (canNerfJump) 
			jumpCanceled = true;
		if (executeDelayJump)
		{
			PerformJump(false);
			executeDelayJump = false;
			jumpBufferActive = false;
		}
		
	}
	private void HandleAirborne()
	{
		isGrounded = false;
		animator.SetBool("IsJumping", !isGrounded);
	}
#endregion

#region Unity Functions
	//Unity Functions
	private void Update()
	{
		if (canMove) 
		{
			//CoyoteTime
			CoyoteTime();
			
			//JumpBuffer
			if (CheckGroundDistance() <= groundDistance)
				canJumpBuffer = true;
			else
				canJumpBuffer = false;
			if(canJumpBuffer)
				JumpBuffer();
			//JumpCounter
			if (canJumpCounter && !canNerfJump)
				pressTime += Time.deltaTime;
			if (jumpCanceled)
			{
				canceledJumpTimer += Time.deltaTime;
				if (canceledJumpTimer >= pressTime)
				{
					canNerfJump = false;
					jumpCanceled = false;
					canceledJumpTimer = 0;
					PerformJump(true);
				}
			}
		}
	}
	private void FixedUpdate()
	{
		if (canMove)
		{
			//Dash
			if (isDashing)
				return;
			//Gravidade
			Gravity();
			//Detecção de bordas
			EdgeDetection();
			//Movimento
			AirControl();
			//Animação
			animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
			animator.SetFloat("yVelocity", rb.velocity.y);
		}
		else 
		{
			rb.gravityScale = 0;
			rb.velocity = new Vector2(0, 0);
		}
	}
#endregion

#region Funções
	//Funções
	//Pulo
	private void PerformJump(bool modifier)
	{
		//Pulo com modificador
		if (modifier)
		{
			//Debug.Log("Jump canceled");
			isJumping = false;
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slowMultiplier);
		}
		else
		{
			//Debug.Log("Jump");
			rb.velocity = new Vector2(rb.velocity.x, jumpPower);
			isJumping = true;
		}
	}

	public void AirControl() 
	{
		if (isJumping)
			rb.velocity = new Vector2(horizontal * speed * airControl, rb.velocity.y);
		else
			rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
	}
	
	//Lógica do Dash
	IEnumerator DashLogic()
	{
		canDash = false;
		isDashing = true;
		animator.SetBool("IsDashing", true);
		rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0);
		tr.emitting = true;
		yield return new WaitForSeconds(dashingTime);
		tr.emitting = false;
		isDashing = false;
		animator.SetBool("IsDashing", false);
		yield return new WaitForSeconds(dashingCooldown);
		canDash = true;
	}
	
	//Modificador de gravidade
	private void Gravity()
	{
		if(canMove) 
		{
			if (!isDashing)
			{
				if (rb.velocity.y < 0)
				{
					//Gravidade
					rb.gravityScale = baseGravity * fallSpdMultiplier;
					rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpd, Mathf.Infinity));
					
					//Pulo
					isJumping = false;
					canJumpCounter = false;
				}
				else
					rb.gravityScale = baseGravity;
			}
			else 
			{
				rb.gravityScale = 0;
			}
		}
	}
	//Verifica a distancia do chão
	private float CheckGroundDistance()
	{
		Vector2 pos1 = transform.position, pos2 = transform.position;
		
		// Lança um Raycast 2D para baixo a partir da posição do personagem
		pos1.x += sprRenderer.bounds.size.x/2;
		RaycastHit2D hit = Physics2D.Raycast(pos1, Vector2.down, jumpPower, playerEvents.groundLayer);
#if UNITY_EDITOR
		//if (hit.collider != null)
			//Debug.Log("Distância 1 até o chão: " + hit.distance);
#endif
		// Lança outro Raycast 2D para baixo a partir da posição do personagem
		pos2.x -= sprRenderer.bounds.size.x/2;
		RaycastHit2D hit2 = Physics2D.Raycast(pos2, Vector2.down, jumpPower, playerEvents.groundLayer);
#if UNITY_EDITOR
		//if (hit.collider != null)
			//Debug.Log("Distância 2 até o chão: " + hit2.distance);
#endif

		float finalDistance = hit.distance > hit2.distance ? hit2.distance : hit.distance;
#if UNITY_EDITOR
		//Debug.Log("Distância até o chão: " + finalDistance);
		//Desenhar os Raycasts para depuração
		Debug.DrawRay(pos1, Vector2.down * hit.distance, Color.red);
		Debug.DrawRay(pos2, Vector2.down * hit2.distance, Color.red);
#endif
		return finalDistance;
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
#endregion

#region GameFeel
	//Gamefeel

	//Chance de pular mesmo após sair do chão
	private void CoyoteTime()
	{
		if(!isGrounded)
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
	//Apertar para pular antecipadamente
	public void JumpBuffer()
	{
		if (!isGrounded)
		{
			if (jumpBufferTimeCounter > 0)
			{
				jumpBufferTimeCounter -= Time.deltaTime;
				if (jumpBufferActive)
				{
					executeDelayJump = true;
				}
			}
			else
			{
				jumpBufferTimeCounter = 0;
			}
		}
	}
	//Detecta se o player está em uma borda
	private void EdgeDetection()
	{
		bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.up, 0.5f, ~PlayerLayer);

		// Hit a Ceiling
		if (ceilingHit) frameVelocity.y = Mathf.Min(0, frameVelocity.y);
	}
#endregion
}
