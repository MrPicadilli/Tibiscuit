using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour {
    protected new Collider collider;

    private void Start() {
        collider = GetComponent<Collider>();
    }
    public void Activate(){
        collider.enabled = true;
    }

    public void Desactivate(){
        collider.enabled = false;
    }
    
}
