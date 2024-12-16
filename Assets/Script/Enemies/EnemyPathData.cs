using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPathData", menuName = "Game/EnemyPathData")]
public class EnemyPathData : ScriptableObject
{
    public bool isCyclic;

    public List<Vector2> anchors;
}