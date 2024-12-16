using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData")]
public class LevelData : ScriptableObject
{
    public int Id;
    public ArrayInt[] mArray;
}