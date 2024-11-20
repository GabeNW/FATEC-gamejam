using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
	public List<string> completedLevels;
}

public class SaveSystem
{
	private static SaveData saveData = new SaveData();
	public static string SaveFileName() 
	{
		string saveFile = Application.persistentDataPath + "/saveData.json";
		return saveFile;
	}
	
	public static void Save(LevelManagerData levelManagerData)
	{
		HandleSaveData(levelManagerData);
		string saveContent = JsonUtility.ToJson(saveData, true);
		File.WriteAllText(SaveFileName(), saveContent);
	}
	
	private static void HandleSaveData(LevelManagerData levelManagerData)
	{
		saveData.completedLevels = new List<string>();

		//Percorre todos os níveis e adiciona ao saveData se estiverem completados
		foreach (var levelData in levelManagerData.levelList)
		{
			if (levelData.completed)
			{
				saveData.completedLevels.Add(levelData.levelName);
				GameManager.Instance.levelsCleared++;
			}
		}
	}
	
	public static void Load(LevelManagerData levelManagerData)
	{
		if (File.Exists(SaveFileName()))
		{
#if UNITY_EDITOR
			Debug.Log("Arquivo encontrado!");
#endif
			string saveContent = File.ReadAllText(SaveFileName());
			saveData = JsonUtility.FromJson<SaveData>(saveContent);
			HandleLoadData(levelManagerData);
		}
		else 
		{
#if UNITY_EDITOR
			Debug.Log("Arquivo não encontrado");
#endif
			foreach (var levelData in levelManagerData.levelList)
			{
				levelData.completed = false;
			}
		}
	}

	private static void HandleLoadData(LevelManagerData levelManagerData)
	{
		levelManagerData.Initialize(); // Certifica-se de que o dicionário está inicializado

		// Marca os níveis como completados com base nos dados salvos
		foreach (string completedLevelName in saveData.completedLevels)
		{
			if (levelManagerData.GetLevelData(completedLevelName) is LevelData levelData)
			{
				levelData.completed = true;
				GameManager.Instance.levelsCleared++;
			}
		}
	}
}
