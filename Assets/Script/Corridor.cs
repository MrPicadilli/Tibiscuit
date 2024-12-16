using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : MonoBehaviour
{
    public Collider endLevelCollider;
    public Collider StartLevelCollider;
    public void OnCorridorTrigger(Collider triggeredCollider)
    {
        if (triggeredCollider == endLevelCollider)
        {
            Debug.Log("Player entered the start of the corridor");
            
            GameManager.Instance.StartNextLevelTransition();
        }
        else if (triggeredCollider == StartLevelCollider)
        {
            Debug.Log("Player exited the end of the corridor");
            // Trigger animation or logic for the end of the corridor
        }
    }
}
