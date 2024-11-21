using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "ScriptableOBJ/Data Containers/Scene Data")]
public class ScenesData : ScriptableObject
{
	public string menuScene;
	public string loadingScene;
	public string firstLevelScene;
	public string endScene;
}
