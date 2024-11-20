using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
	[Header("Dependencies")]
	private LevelManager worldSwitcher;
	private InputReader inputReader;
	
	//Input Reader
	public void InitializeInput(InputReader inputReader)
	{
		this.inputReader = inputReader;
	}
	
	private void OnEnable()
	{
		inputReader.SwitchUpEvent += OnSwitchUp;
		inputReader.SwitchDownEvent += OnSwitchDown;	
	}
	
	private void OnDisable()
	{
		inputReader.SwitchUpEvent -= OnSwitchUp;
		inputReader.SwitchDownEvent -= OnSwitchDown;
	}

	private void OnSwitchUp()
	{
		worldSwitcher.SwitchWorldUp();
	}

	private void OnSwitchDown()
	{
		worldSwitcher.SwitchWorldDown();
	}
	
	void Start()
	{
		GameObject levelManager = GameManager.Instance.FindGameObject("Level Manager");
		if (levelManager != null)
			worldSwitcher = levelManager.GetComponent<LevelManager>();
#if UNITY_EDITOR
		//else 
			//Debug.LogError("Level Manager not found! PlayerSwitch script will not work!");
#endif
	}
}
