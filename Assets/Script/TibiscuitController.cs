using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TibiscuitController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] 
    private float speed = 1.0f;
    private float inputX;
    private float inputY;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        Vector3 direction  = new Vector3(inputX,0,inputY);
        controller.Move(direction * speed* Time.deltaTime);
    }
}
