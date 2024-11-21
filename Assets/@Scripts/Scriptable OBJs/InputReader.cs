using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "ScriptableOBJ/Inputs/Input Reader")]
public class InputReader : ScriptableObject
{
	[SerializeField] private InputActionAsset playerInputs;
	
	public event UnityAction<Vector2> MoveEvent;
	public event UnityAction JumpEvent;
	public event UnityAction JumpCanceledEvent;
	public event UnityAction DashEvent;
	public event UnityAction SwitchUpEvent;
	public event UnityAction SwitchDownEvent;
	
	public event UnityAction BackToMenuEvent;
	
	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction dashAction;
	private InputAction switchUpAction;
	private InputAction switchDownAction;
	
	private InputAction backToMenuAction;
	
	private void OnEnable()
	{
		moveAction = playerInputs.FindAction("Move");
		jumpAction = playerInputs.FindAction("Jump");
		dashAction = playerInputs.FindAction("Dash");
		switchUpAction = playerInputs.FindAction("SwitchDimensionUp");
		switchDownAction = playerInputs.FindAction("SwitchDimensionDown");
		
		backToMenuAction = playerInputs.FindAction("BackToMenu");
		
		moveAction.started += OnMove;
		moveAction.performed += OnMove;
		moveAction.canceled += OnMove;
		
		jumpAction.started += OnJump;
		jumpAction.performed += OnJump;
		jumpAction.canceled += OnJump;
		
		dashAction.started += OnDash;
		dashAction.performed += OnDash;
		dashAction.canceled += OnDash;
		
		switchUpAction.started += OnSwitchUp;
		switchUpAction.performed += OnSwitchUp;
		switchUpAction.canceled += OnSwitchUp;
		
		switchDownAction.started += OnSwitchDown;
		switchDownAction.performed += OnSwitchDown;
		switchDownAction.canceled += OnSwitchDown;
		
		backToMenuAction.started += OnBackToMenu;
		backToMenuAction.performed += OnBackToMenu;
		backToMenuAction.canceled += OnBackToMenu;
		
		moveAction.Enable();
		jumpAction.Enable();
		dashAction.Enable();
		switchUpAction.Enable();
		switchDownAction.Enable();
		
		backToMenuAction.Enable();
	}

	private void OnDisable() 
	{
		moveAction.started -= OnMove;
		moveAction.performed -= OnMove;
		moveAction.canceled -= OnMove;
		
		jumpAction.started -= OnJump;
		jumpAction.performed -= OnJump;
		jumpAction.canceled -= OnJump;
		
		dashAction.started -= OnDash;
		dashAction.performed -= OnDash;
		dashAction.canceled -= OnDash;
		
		switchUpAction.started -= OnSwitchUp;
		switchUpAction.performed -= OnSwitchUp;
		switchUpAction.canceled -= OnSwitchUp;
		
		switchDownAction.started -= OnSwitchDown;
		switchDownAction.performed -= OnSwitchDown;
		switchDownAction.canceled -= OnSwitchDown;
		
		backToMenuAction.started -= OnBackToMenu;
		
		moveAction.Disable();
		jumpAction.Disable();
		dashAction.Disable();
		switchUpAction.Disable();
		switchDownAction.Disable();
		
		backToMenuAction.Disable();
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		MoveEvent?.Invoke(context.ReadValue<Vector2>());
	}
	
	private void OnJump(InputAction.CallbackContext context)
	{
		if (JumpEvent != null && context.performed)
			JumpEvent.Invoke();
		if (JumpEvent != null && context.canceled)
			JumpCanceledEvent.Invoke();
	}

	private void OnSwitchUp(InputAction.CallbackContext context)
	{
		if (SwitchUpEvent != null && context.started)
			SwitchUpEvent.Invoke();
	}
	
	private void OnSwitchDown(InputAction.CallbackContext context)
	{
		if (SwitchDownEvent != null && context.started)
			SwitchDownEvent.Invoke();
	}

	private void OnDash(InputAction.CallbackContext context)
	{
		if (DashEvent != null && context.started)
			DashEvent.Invoke();
	}
	
	private void OnBackToMenu(InputAction.CallbackContext context)
	{
		if (BackToMenuEvent != null && context.started)
			BackToMenuEvent.Invoke();
	}
	
}
