using UnityEngine;

public class MySceneManager : MonoBehaviour
{
	[HideInInspector] public string currentScene;

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
