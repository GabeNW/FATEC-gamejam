using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScritableOBJ/Data Containers/Level Data")]
public class LevelData : ScriptableObject
{
    //Características do level
    public UnityEditor.SceneAsset scene;
    public string levelName;
    public int collectables = 1;
    public const int dimensions = 3;
    public int dimensionsAvailable = 2;
    public int startingDimension = 1;
    public bool thirdDim = false;
    public bool canDash = false;

    // Método para definir automaticamente o nome da cena a partir do SceneAsset no Editor
    private void OnValidate()
    {
        if (scene != null)
        {
            levelName = scene.name;
        }
    }
}
