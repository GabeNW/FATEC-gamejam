using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "ScritableOBJ/Data Containers/Scene Data")]
public class ScenesData : ScriptableObject
{
    //Lista de todas as cenas
    [SerializeField] private List<UnityEditor.SceneAsset> allSceneList;
    //Dicion�rio que mapear� as cenas
    private Dictionary<string, UnityEditor.SceneAsset> sceneDataDictionary;

    //Lista de Cenas importantes
    [SerializeField] private List<UnityEditor.SceneAsset> coreScenesList;
    public string menuScene;
    public string loadingScene;
    public string firstLevelScene;

    public void Initialize()
    {
        //Cria o dicion�rio se ele n�o existir
        if (sceneDataDictionary == null || sceneDataDictionary.Count == 0)
        {
            sceneDataDictionary = new Dictionary<string, UnityEditor.SceneAsset>();
        }
        //Preenche automaticamente o dicion�rio com os dados da lista
        foreach (UnityEditor.SceneAsset sceneData in allSceneList)
        {
            if (!sceneDataDictionary.ContainsKey(sceneData.name))
            {
                sceneDataDictionary.Add(sceneData.name, sceneData);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"O n�vel {sceneData.name} j� est� no dicion�rio.");
#endif
            }
        }
    }

    //Fun��o para acessar os dados da cena por nome
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
            Debug.LogError($"N�vel '{sceneName}' n�o encontrado!");
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
