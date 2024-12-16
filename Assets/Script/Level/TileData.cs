using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Levels/TileData")]
public class TileData : ScriptableObject
{
    public int Id;
    public GameObject prefab;
}