using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
	[Header("GroundCheck")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Vector3 groundCheckSize = new Vector3(1, 0.5f, 1);
	[SerializeField] private Color32 gizmosColor = Color.green;
	public LayerMask groundLayer;
	
	//Events
	public event UnityAction onGround;
	public event UnityAction onAir;
	private bool wasGrounded;
	public bool IsGrounded { get; private set; }
	
	private void Update()
	{
		//Verifica o estado atual do chão
		bool isGrounded = CheckGrounded();

		if (isGrounded && !wasGrounded)
			onGround?.Invoke();
		else if (!isGrounded && wasGrounded)
			onAir?.Invoke();
			
		//Atualiza o estado
		IsGrounded = isGrounded;
		wasGrounded = isGrounded;
	}

	private bool CheckGrounded()
	{
		return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.2f, groundLayer);
	}
	
	//Desenha no Gizmos
	private void OnDrawGizmos()
	{
		if (groundCheck != null && groundCheckSize != Vector3.zero) 
		{	
			//Desenha a caixa de colisão do chão
			Gizmos.color = gizmosColor;
			Gizmos.DrawCube(groundCheck.position, groundCheckSize);
		}
	}
}
