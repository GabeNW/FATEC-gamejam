using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Manager Data", menuName = "ScritableOBJ/Data Containers/Level Manager Data")]
public class LevelManagerData : ScriptableObject
{
    //Lista dos ScriptableObjects
    [SerializeField] private List<LevelData> levelList;
    //Dicionário que mapeará o nome da cena aos ScriptableObjects
    private Dictionary<string, LevelData> levelDataDictionary;

    public void Initialize()
    {
        //Cria o dicionário se ele não existir
        if (levelDataDictionary == null || levelDataDictionary.Count == 0)
        {
            levelDataDictionary = new Dictionary<string, LevelData>();
        }
        //Preenche automaticamente o dicionário com os dados da lista
        foreach (LevelData levelData in levelList)
        {
            if (!levelDataDictionary.ContainsKey(levelData.levelName))
            {
                levelDataDictionary.Add(levelData.levelName, levelData);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"O nível {levelData.levelName} já está no dicionário.");
#endif
            }
        }
    }

    //Função para acessar os dados de um level pelo nome
    public LevelData GetLevelData(string levelName)
    {
        if (levelDataDictionary == null)
        {
#if UNITY_EDITOR
            Debug.Log("Inicializando o Scriptable Obj");
#endif
            Initialize();
        }

        if (levelDataDictionary.TryGetValue(levelName, out LevelData levelData))
        {
            return levelData;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Nível '{levelName}' não encontrado!");
#endif
            return null;
        }
    }

    //Função para acessar o primeiro Level
    public LevelData GetFirstLevel()
    {
        if (levelDataDictionary == null)
        {
#if UNITY_EDITOR
            Debug.Log("Inicializando o Scriptable Obj");
#endif
            Initialize();
        }
        if (levelDataDictionary.TryGetValue("Level1", out LevelData levelData))
        {
            return levelData;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Nível 1 não encontrado!");
#endif
            return null;
        }
    }

}
