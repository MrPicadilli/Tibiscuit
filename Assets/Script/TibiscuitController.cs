using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TibiscuitController : MonoBehaviour
{

    [SerializeField] private PlayerStateData normalStateData;
    [SerializeField] private PlayerStateData dryStateData;
    [SerializeField] private PlayerStateData humidStateData;
    public static TibiscuitController Instance;

    private CharacterController controller;
    public float speed = 1.0f;
    private float inputX;
    private float inputY;
    private Renderer playerRenderer;
    public PlayerState currentState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
        ChangeState(new NormalState(normalStateData));
        
        //playerRenderer.material = currentState.GetMaterial();
        
    }
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        speed = newState.GetSpeed();
        playerRenderer.material = currentState.GetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    public void Move(){
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(inputX, 0, inputY);
        controller.Move(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("collision with " + other.gameObject.name + " layer " + other.gameObject.layer);
        Debug.Log(" vent : " + LayerMask.NameToLayer("Ventilation") + " water : " + LayerMask.NameToLayer("Water"));
        if (other.gameObject.layer == LayerMask.NameToLayer("Ventilation")){
            Debug.Log("Ventilation");
            ChangeState(new DryState(dryStateData));
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Water")){
            Debug.Log("Water");
            ChangeState(new HumidState(humidStateData));
        }
    }
}
