using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Todas as cenas
    [SerializeField] private ScenesData scenesData;

    //Override do método Awake do singleton
    protected override void Awake()
    {
        base.Awake();
        scenesData.Initialize();
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == scenesData.loadingScene)
            LoadScene(scenesData.menuScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Função para retornar o nome da cena atual
    public string CurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == scenesData.firstLevelScene)
        {
#if UNITY_EDITOR
            Debug.Log("First Level Scene Loaded");
#endif

        }
        return activeScene.name;
    }

    //Função para buscar GameObjects (até os desativados)
    private GameObject FindGameObject(string name)
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

    //Função para carregar cenas
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    //Função para carregar o primeiro nível
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(scenesData.firstLevelScene);
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
}
