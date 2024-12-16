using UnityEngine;

public class CorridorCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Ensure only the player triggers this
        {
            transform.parent.GetComponent<Corridor>().OnCorridorTrigger(this.GetComponent<Collider>());
        }
    }

}