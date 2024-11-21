using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private LevelManagerData levelManagerData;
	[SerializeField] private List<GameObject> tileMapObjects;
	[HideInInspector] public LevelData currentLevel;
	//Animação de mudança de nível
	[SerializeField] public Animator transitionAnim;

	private int currentWorld = 0;
	private void OnEnable(){
		GameManager.Instance.onLevelClear+=NextLevel;
	}

	private void OnDisable(){
		GameManager.Instance.onLevelClear-=NextLevel;
	}
    private void NextLevel()
    {
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition(){
		transitionAnim.SetTrigger("end");
		yield return new WaitForSeconds(1.5f);
		GameManager.Instance.NextLevel();
	}

	private void Awake()
	{
		levelManagerData.Initialize();
		CurrentSceneData();
		SwitchWorld(currentWorld); 
	}
	private void CurrentSceneData()
	{
		currentLevel = levelManagerData.GetLevelData(GameManager.Instance.CurrentScene());
		currentWorld = currentLevel.startingDimension - 1;
	}
	
	//Função para alternar para a próxima dimensão (sentido 1)
	public void SwitchWorldUp()
	{
		if (!(currentWorld + 1 >= currentLevel.dimensionsAvailable))
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
			currentWorld = currentLevel.dimensionsAvailable - 1;
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
