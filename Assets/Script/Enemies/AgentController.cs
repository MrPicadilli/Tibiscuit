using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AgentController : MonoBehaviour
{

    public Camera cam;

    public NavMeshAgent agent;
    public EnemyPathData enemyPathData;

    public bool hasSeenPlayer = false;

    public float waypointTolerance = 0.5f;

    public int currentWaypointIndex = 0;
    public bool isForth = true;
    public bool isDebug = false;



    private void Awake()
    {
        cam = Camera.main;
    }
    void Start()
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned.");
            return;
        }
        agent.updateRotation = false;
        if (enemyPathData != null)
        {
            // Start moving to the first waypoint
            MoveToWaypoint();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (isDebug)
        {
            MovebyMouse();
        }
        else
        {
            if (enemyPathData.isCyclic)
            {
                MoveCyclicly();

            }
            else
            {
                MoveBackAndForth();

            }
        }

        FaceTarget();

    }

    void MoveBackAndForth()
    {
        if (agent == null || enemyPathData.anchors.Count == 0)
            return;

        // Check if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            if (isForth)
            {

                if (currentWaypointIndex + 1 > enemyPathData.anchors.Count - 1)
                {
                    currentWaypointIndex = currentWaypointIndex - 1;
                    isForth = false;
                }
                else
                {
                    currentWaypointIndex = currentWaypointIndex + 1;
                }
            }
            else
            {
                if (currentWaypointIndex - 1 < 0)
                {
                    currentWaypointIndex = currentWaypointIndex + 1;
                    isForth = true;
                }
                else
                {
                    currentWaypointIndex = currentWaypointIndex - 1;
                }
            }

            MoveToWaypoint();
        }
    }

    void MoveCyclicly()
    {
        if (agent == null || enemyPathData.anchors.Count == 0)
            return;

        // Check if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % enemyPathData.anchors.Count;
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        // Set a path to the current waypoint
        NavMeshPath path = new NavMeshPath();
        Vector3 targetWaypoint = new Vector3(enemyPathData.anchors[currentWaypointIndex].x, 0, enemyPathData.anchors[currentWaypointIndex].y);
        if (agent.CalculatePath(targetWaypoint, path))
        {
            agent.SetPath(path);
        }
        else
        {
            Debug.LogError("Failed to calculate path to waypoint: " + targetWaypoint);
        }
    }
    void MovebyMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = agent.velocity;
        Vector3 direction2D = new Vector3(direction.x, 0, direction.z);
        if (direction2D != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction2D);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }

    }


    public void PlayerFound(Vector3 lastPositionPlayer)
    {
        hasSeenPlayer = true;
        agent.SetDestination(lastPositionPlayer);
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log(other.gameObject.layer);
    }



}

