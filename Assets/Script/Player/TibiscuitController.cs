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
    public Barrier[] barriers;
    public AirCurrent[] airCurrents;
    public bool isInAnimation = false;


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
        barriers = (Barrier[])FindObjectsOfType(typeof(Barrier));
        airCurrents = (AirCurrent[])FindObjectsOfType(typeof(AirCurrent));
        //playerRenderer.material = currentState.GetMaterial();

    }
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
        speed = newState.GetSpeed();
        playerRenderer.material = currentState.GetMaterial();
        switch (currentState.GetName())
        {
            case "Dry":
                UnlockAirCurrent();
                LockBarrier();
                break;
            case "Humid":
                UnlockBarrier();
                LockAirCurrent();
                break;
            case "Normal":
                LockBarrier();
                LockAirCurrent();
                break;
            default:
                LockBarrier();
                LockAirCurrent();
                break;
        }
    }

    public void UnlockBarrier()
    {
        foreach (Barrier barrier in barriers)
        {
            barrier.Desactivate();
        }
    }
    public void UnlockAirCurrent()
    {
        foreach (AirCurrent airCurrent in airCurrents)
        {
            airCurrent.Desactivate();
        }
    }
    public void LockBarrier()
    {
        foreach (Barrier barrier in barriers)
        {
            barrier.Activate();
        }
    }
    public void LockAirCurrent()
    {
        foreach (AirCurrent airCurrent in airCurrents)
        {
            airCurrent.Activate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (!isInAnimation)
        {
            Move();
        }


    }
    public void StopControl()
    {
        isInAnimation = true;
    }
    public void StartControl()
    {
        isInAnimation = false;
    }

    public void Move()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(inputX, 0, inputY);
        controller.Move(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with " + other.gameObject.name + " layer " + other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ventilation"))
        {
            Debug.Log("Ventilation");
            ChangeState(new DryState(dryStateData));
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Debug.Log("Water");
            ChangeState(new HumidState(humidStateData));
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy");
            GameManager.Instance.GameOver();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
