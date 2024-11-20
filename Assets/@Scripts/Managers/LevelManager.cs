using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private LevelManagerData levelManagerData;
	[SerializeField] private List<GameObject> tileMapObjects;
	[HideInInspector] public LevelData currentLevel;

	private int currentWorld = 0;

	private void Awake()
	{
		levelManagerData.Initialize();
		CurrentSceneData();
	}
	private void CurrentSceneData()
	{
		currentLevel = levelManagerData.GetLevelData(GameManager.Instance.CurrentScene());
		GameManager.Instance.currentLevel = currentLevel;
	}
	
	//Função para alternar para a próxima dimensão (sentido 1)
	public void SwitchWorldUp()
	{
		if (!(currentWorld + 1 >= GameManager.Instance.currentLevel.dimensionsAvailable))
			currentWorld++;
		else
			currentWorld = 0;
		SwitchWorld(currentWorld);
	}

	//Função para alternar para a próxima dimensão (sentido 2)
	public void SwitchWorldDown()
	{
		if (currentWorld > 0)
			currentWorld--;
		else
			currentWorld = GameManager.Instance.currentLevel.dimensionsAvailable - 1;
		SwitchWorld(currentWorld);
	}

	//Função para alternar entre os mundos
	private void SwitchWorld(int target = 0) 
	{
		for(int i = 0; i < tileMapObjects.Count; i++)
		{
			if(i == target)
				tileMapObjects[i].SetActive(true);
			else
				tileMapObjects[i].SetActive(false);
		}
	}
	

}
