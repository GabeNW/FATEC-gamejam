using UnityEngine;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
	[HideInInspector] public string currentScene;

	private void Awake()
	{
		//Verificar a cena atual
		currentScene = GameManager.Instance.CurrentScene();
		if (currentScene == GameManager.Instance.scenesData.menuScene)
		{
			GameObject title = GameManager.Instance.FindGameObject("Tittle");
			title.GetComponent<Text>().text = GameManager.Instance.levelManagerData.GetLevelData(GameManager.Instance.scenesData.firstLevelScene).completed.ToString();
		}
	}

	//Função para carregar uma cena
	public void LoadScene(string sceneName)
	{
		GameManager.Instance.LoadScene(sceneName);
	}

	//Função para carregar o primeiro nível
	public void StartButton()
	{
		GameManager.Instance.StartGame();
	}

	//Função para encerrar o jogo
	public void ExitButton()
	{
		GameManager.Instance.EndGame();
	}
}
