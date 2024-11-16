using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScritableOBJ/Data Containers/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName = "Level";
    public const int dimensions = 3;
    public int dimensionsAvailable = 2;
    public int startingDimension = 1;
    public int scrap = 1;
    public bool canDash = false;
    public bool completed = false;
}
