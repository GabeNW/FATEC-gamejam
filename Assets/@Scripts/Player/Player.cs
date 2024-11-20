using UnityEngine;

[RequireComponent(typeof(PlayerEvents))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerSwitch))]
public class Player : MonoBehaviour
{
	[Header("Input")]
	[SerializeField] private InputReader inputReader;
	[Header("Player Position")]
	[SerializeField] private Vector3 playerPosition;
	
	private PlayerEvents playerEvents;
	private PlayerMovement playerMovement;
	private PlayerSwitch playerSwitch;
	
	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
		playerSwitch = GetComponent<PlayerSwitch>();
		playerEvents = GetComponent<PlayerEvents>();
		
		playerMovement.InitializeInput(inputReader);
		playerMovement.InitializeEvents(playerEvents);
		playerSwitch.InitializeInput(inputReader);
		
		transform.position = playerPosition;
		playerMovement.canMove = true;
	}
}
