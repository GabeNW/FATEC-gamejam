using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelManagerData levelManagerData;
    [HideInInspector] public LevelData currentLevel;

    private void Awake()
    {
        levelManagerData.Initialize();
        CurrentSceneData();
    }

    private void CurrentSceneData()
    {
        currentLevel = levelManagerData.GetLevelData(GameManager.Instance.CurrentScene());
    }


}
