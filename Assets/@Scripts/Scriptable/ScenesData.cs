using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "ScritableOBJ/Data Containers/Scene Data")]
public class ScenesData : ScriptableObject
{
    //Lista de todas as cenas
    [SerializeField] private List<UnityEditor.SceneAsset> allSceneList;
    //Dicionário que mapeará as cenas
    private Dictionary<string, UnityEditor.SceneAsset> sceneDataDictionary;

    //Lista de Cenas importantes
    [SerializeField] private List<UnityEditor.SceneAsset> coreScenesList;
    public string menuScene;
    public string loadingScene;
    public string firstLevelScene;

    public void Initialize()
    {
        //Cria o dicionário se ele não existir
        if (sceneDataDictionary == null || sceneDataDictionary.Count == 0)
        {
            sceneDataDictionary = new Dictionary<string, UnityEditor.SceneAsset>();
        }
        //Preenche automaticamente o dicionário com os dados da lista
        foreach (UnityEditor.SceneAsset sceneData in allSceneList)
        {
            if (!sceneDataDictionary.ContainsKey(sceneData.name))
            {
                sceneDataDictionary.Add(sceneData.name, sceneData);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"O nível {sceneData.name} já está no dicionário.");
#endif
            }
        }
    }

    //Função para acessar os dados da cena por nome
    public UnityEditor.SceneAsset GetLevelData(string sceneName)
    {
        if (sceneDataDictionary == null)
        {
#if UNITY_EDITOR
            Debug.Log("Inicializando o Scriptable Obj");
#endif
            Initialize();
        }

        if (sceneDataDictionary.TryGetValue(sceneName, out UnityEditor.SceneAsset sceneData))
        {
            return sceneData;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Nível '{sceneName}' não encontrado!");
#endif
            return null;
        }
    }

    private void OnValidate()
    {
        if (coreScenesList.Count > 2)
        {
            loadingScene = coreScenesList[0].name;
            menuScene = coreScenesList[1].name;
            firstLevelScene = coreScenesList[2].name;
        }
    }

}
