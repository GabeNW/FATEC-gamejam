using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[Header("Important Data")]
	//Todas as cenas
	public ScenesData scenesData;
	//Informações dos levels
	public LevelManagerData levelManagerData;
	
	//Informações do level atual
	[HideInInspector] public LevelData currentLevel;
	
	private const int levels = 4;
	public bool canSave = false;
	private int scrapCollected = 0;
	public int levelsCleared = 0;
	private int currentLevelIndex = 0;

	//Override do método Awake do singleton
	protected override void Awake()
	{
		base.Awake();
		SaveSystem.Load(levelManagerData);
		currentLevelIndex = levelsCleared;
		Scene activeScene = SceneManager.GetActiveScene();
		if (activeScene.name == scenesData.loadingScene)
			LoadScene(scenesData.menuScene);
		scrapCollected = 0;
	}

	public void OnBackToMenu()
	{
		scrapCollected = 0;
		if (CurrentScene() != scenesData.menuScene)
			LoadScene(scenesData.menuScene);
	}
	
	private void LevelClear()
	{
#if UNITY_EDITOR
		Debug.Log("Level Cleared");
#endif
		currentLevel.completed = true;
#if UNITY_EDITOR
		if(canSave)
#endif
			SaveSystem.Save(levelManagerData);
		NextLevel();
	}
	
	//Função para verificar se a cena existe
	private bool SceneExists(string sceneName) 
	{
		int sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
		return sceneIndex != -1;
	}

	//Função para retornar o nome da cena atual
	public string CurrentScene()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		if (activeScene.name.Contains("Level"))
		{
			currentLevel = levelManagerData.GetLevelData(activeScene.name);
			char temp = activeScene.name[activeScene.name.Length-1];
			currentLevelIndex = int.Parse(temp.ToString());
		}
		return activeScene.name;
	}
	
	public void Restart() 
	{
		//Transition
		scrapCollected = 0;
		LoadScene(CurrentScene());
	}
	
	public void NextLevel()
	{
		scrapCollected = 0;
#if UNITY_EDITOR
		//Debug.Log("Current Level Index: " + currentLevelIndex + "| Level Cleared: " + levelsCleared + "| Level List Count: " + levelManagerData.levelList.Count);
#endif
		if(currentLevelIndex == levelManagerData.levelList.Count)
			LoadScene(scenesData.endScene);
		else
			LoadScene("Level" + ++currentLevelIndex);
#if UNITY_EDITOR
		//Debug.Log("Current Level Index: " + currentLevelIndex + "| Level Cleared: " + levelsCleared + "| Level List Count: " + levelManagerData.levelList.Count);
#endif
	}
	
	public bool AddCollected()
	{
#if UNITY_EDITOR
		//Debug.Log("Scrap before: " + scrapCollected);
#endif
		if (scrapCollected < currentLevel.scrap)
		{
			scrapCollected++;
#if UNITY_EDITOR
			//Debug.Log("Scrap after: " + scrapCollected);
			//Debug.Log("Level Scrap: " + currentLevel.scrap);
#endif
			if(scrapCollected == currentLevel.scrap)
				LevelClear();
			return true;
		}
		else
			return false;
	}
	
	//Função para carregar cenas
	public void LoadScene(string sceneName)
	{
		if (SceneExists(sceneName))
		{
			SceneManager.LoadSceneAsync(sceneName);
		}
#if UNITY_EDITOR
		else
			Debug.LogError($"A cena '{sceneName}' não existe ou não foi adicionada à build.");
#endif
	}

	//Função para carregar o primeiro nível
	public void StartGame()
	{
		if (levelsCleared == 0)
			SceneManager.LoadSceneAsync(scenesData.firstLevelScene);
		else if (levelsCleared >= levels) 
		{
			levelsCleared = levels;
#if UNITY_EDITOR
			Debug.Log("All levels cleared");
#endif
			currentLevelIndex = 0;
			LoadScene("Level" + ++currentLevelIndex);
		}
		else
			LoadScene("Level"+(currentLevelIndex+1));
	}
	
	public void BackToMenu()
	{
		if (CurrentScene() == scenesData.endScene)
			LoadScene(scenesData.menuScene);
	}

	//Função para encerrar o jogo
	public void EndGame()
	{
#if UNITY_EDITOR
		Debug.Log("Input de fim da aplicação recebido");
#else
		Application.Quit();
#endif
	}
	
	//Função para buscar GameObjects (até os desativados)
	public GameObject FindGameObject(string name)
	{
		List<GameObject> GetAllObjectsOnlyInScene()
		{
			List<GameObject> objectsInScene = new List<GameObject>();
			foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
			{
#if UNITY_EDITOR
				if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
#else
				if (!(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))   
#endif
					objectsInScene.Add(go);
			}
			return objectsInScene;
		}

		List<GameObject> objectsInScene = GetAllObjectsOnlyInScene();
		GameObject obj = objectsInScene.Find(obj => obj.name == name);
		return obj;
	}
}
