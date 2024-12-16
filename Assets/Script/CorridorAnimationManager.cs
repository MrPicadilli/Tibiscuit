using UnityEngine;
using System.Collections;

public class CorridorAnimationManager : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public Transform cameraTransform; // Reference to the camera
    public Transform corridorCenter;  // The center point of the corridor
    public Transform nextLevelStartPlayer; // The start point of the next level
    public Transform nextLevelStartCamera; // The start point of the next level
    public UIManager uiManager;       // Reference to the UI manager script for Tibiscuit and Nana

    public float cameraMoveSpeed = 5f;  // Speed at which the camera moves
    public float playerRunSpeed = 5f;   // Speed at which the player runs
    public float environmentMoveSpeed = 10f; // Speed for environment moving
    public float waitTimeBeforeNext = 2f;    // Wait time before transitioning to the next level

    private bool isEnvironmentMoving = false;
    private void Start()
    {
        cameraTransform = Camera.main.transform;

    }

    public void StartAnimation()
    {
        StartCoroutine(AnimationSequenceCorridor());
    }

    private IEnumerator AnimationSequenceCorridor()
    {
        // Step 1: Move the camera to the level of the player
        while (Vector3.Distance(new Vector3(cameraTransform.position.x, 0, cameraTransform.position.z), new Vector3(player.position.x, 0, player.position.z)) > 0.1f)
        {
            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position,
                new Vector3(player.position.x, cameraTransform.position.y, player.position.z),
                cameraMoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Step 2: Player runs to the center of the corridor, camera follows
        while (Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(corridorCenter.position.x, 0, corridorCenter.position.z)) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position,
                new Vector3(corridorCenter.position.x, player.position.y, corridorCenter.position.z),
                playerRunSpeed * Time.deltaTime);

            cameraTransform.position = new Vector3(player.position.x,
                cameraTransform.position.y,
                player.position.z); // Move camera along with player
            yield return null;
        }
        // step3
        LevelGen.Instance.DeleteLevel();
        // Step 3: Stop the player and camera at the center, but keep running animation
        //player.GetComponent<Animator>().SetBool("isRunning", true); // Ensure the running animation is active
        // Freeze movement but keep animation running
        yield return new WaitForSeconds(1f);

        // Step 4: Make the environment move like a running course
        isEnvironmentMoving = true;
        StartCoroutine(MoveEnvironment());
        yield return new WaitForSeconds(3f); // Simulate the environment moving for 3 seconds
        isEnvironmentMoving = false;

        // Step 5: Activate UI with Discussion
        uiManager.ShowDiscussionPanel();
        yield return new WaitForSeconds(2f); // Allow time for UI animation
        uiManager.HideDiscussionPanel();
        // step 6 : 
        LevelGen.Instance.AdvanceToNextLevel();
        LevelGen.Instance.GenerateGround();

        // Step 7: Move the camera and player to the start of the next level
        while (Vector3.Distance(player.position, nextLevelStartPlayer.position) > 0.1f || Vector3.Distance(cameraTransform.position, nextLevelStartCamera.position) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position,
                nextLevelStartPlayer.position,
                playerRunSpeed * Time.deltaTime);

            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position,
                nextLevelStartCamera.position,
                cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        // Step 6: Start the creation of level with animation 
        

        LevelGen.Instance.GenerateLevel();
        // Step 8: give back control to the player
        TibiscuitController.Instance.StartControl();
        Debug.Log("Animation sequence complete.");
        GameManager.Instance.isInAnimation = false;
    }

    private IEnumerator MoveEnvironment()
    {
        while (isEnvironmentMoving)
        {
            // Simulate environment motion, e.g., scrolling textures or objects
            // Example: Move all environment objects backward
            foreach (GameObject envObject in GameObject.FindGameObjectsWithTag("Environment"))
            {
                envObject.transform.position -= new Vector3(0, 0, environmentMoveSpeed * Time.deltaTime);
            }
            yield return null; // Wait for the next frame
        }
    }
}