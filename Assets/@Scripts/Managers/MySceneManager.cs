using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    [HideInInspector] public string currentScene;

    private void Awake()
    {
        //Verificar a cena atual
        currentScene = GameManager.Instance.CurrentScene();
    }

    //Fun��o para carregar uma cena
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    //Fun��o para carregar o primeiro n�vel
    public void StartButton()
    {
        GameManager.Instance.StartGame();
    }

    //Fun��o para encerrar o jogo
    public void ExitButton()
    {
        GameManager.Instance.EndGame();
    }
}
