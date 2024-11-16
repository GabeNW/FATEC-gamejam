using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Manager Data", menuName = "ScritableOBJ/Data Containers/Level Manager Data")]
public class LevelManagerData : ScriptableObject
{
    //Lista dos ScriptableObjects
    [SerializeField] private List<LevelData> levelList;
    //Dicion�rio que mapear� o nome da cena aos ScriptableObjects
    private Dictionary<string, LevelData> levelDataDictionary;

    public void Initialize()
    {
        //Cria o dicion�rio se ele n�o existir
        if (levelDataDictionary == null || levelDataDictionary.Count == 0)
        {
            levelDataDictionary = new Dictionary<string, LevelData>();
        }
        //Preenche automaticamente o dicion�rio com os dados da lista
        foreach (LevelData levelData in levelList)
        {
            if (!levelDataDictionary.ContainsKey(levelData.levelName))
            {
                levelDataDictionary.Add(levelData.levelName, levelData);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"O n�vel {levelData.levelName} j� est� no dicion�rio.");
#endif
            }
        }
    }

    //Fun��o para acessar os dados de um level pelo nome
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
            Debug.LogError($"N�vel '{levelName}' n�o encontrado!");
#endif
            return null;
        }
    }

    //Fun��o para acessar o primeiro Level
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
            Debug.LogError("N�vel 1 n�o encontrado!");
#endif
            return null;
        }
    }

}
