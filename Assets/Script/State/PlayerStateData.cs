using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateData", menuName = "States/PlayerStateData")]
public class PlayerStateData : ScriptableObject
{
    public string stateName;
    public Material stateMaterial;
    public float stateSpeed;
}