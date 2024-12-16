using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
public class LevelGen : MonoBehaviour
{

    public List<LevelData> levelDatas;
    public List<TileData> tileDatas;
    public List<EnemyPathData> enemyPathDatas;
    public LevelData actualLevelData;
    public Vector3 offsetMap = new Vector3(0, 0, 0);
    public GameObject groundPrefab;
    public NavMeshSurface surface;
    public int width = 11;
    public int height = 11;
    public float animationSpeed = 5.0f;
    public float offsetAnimation = 2.0f;
    public float waitTimeEachTile = 0.1f;
    public float waitTimeAfterGround = 3.0f;


    public static LevelGen Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    // Use this for initialization
    void Start()
    {
        actualLevelData = levelDatas[0];
        GenerateGround();
        GenerateLevel();


    }
    public void LevelComplete()
    {

    }



    // Create a grid based level
    public void GenerateLevel()
    {
        StartCoroutine(AnimationSequenceTile());
    }
    public void GenerateGround()
    {
        StartCoroutine(AnimationSequenceGround());
    }
    private IEnumerator AnimationSequenceGround()
    {
        // Step 1 : create ground
        GameObject ground = Instantiate(groundPrefab, new Vector3(groundPrefab.transform.position.x, groundPrefab.transform.position.y - offsetAnimation, groundPrefab.transform.position.z) + offsetMap, Quaternion.identity, transform);

        while (Mathf.Abs(Vector3.Distance(ground.transform.position, new Vector3(ground.transform.position.x, groundPrefab.transform.position.y, ground.transform.position.z))) > 0.01f)
        {
            ground.transform.position = Vector3.MoveTowards(ground.transform.position,
                new Vector3(ground.transform.position.x, groundPrefab.transform.position.y, ground.transform.position.z),
                animationSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        yield return new WaitForSeconds(waitTimeAfterGround);
    }
    private IEnumerator AnimationSequenceTile()
    {


        Debug.Log("start animation");
        ArrayInt[] arrayInt = actualLevelData.mArray;
        for (int id = 1; id < tileDatas.Count; id++)
        {
            if (id >= 6)
            {
                surface.BuildNavMesh();
            }
            for (int i = 0; i < arrayInt.Length; i++)
            {
                for (int y = 0; y < arrayInt[i].Length; y++)
                {
                    if (arrayInt[i][y] == id)
                    {
                        StartCoroutine(CreateTile(i, y, arrayInt[i][y]));
                        yield return new WaitForSeconds(waitTimeEachTile);
                    }

                }
            }
        }
        yield return null;
    }

    public IEnumerator CreateTile(int x, int y, int tileType)
    {
        GameObject tilePrefab = GetTilePrefab(tileType);
        if (tilePrefab == null)
        {
            yield return null;
        }

        if (tilePrefab != null)
        {
            if (tilePrefab.GetComponent<AgentController>())
            {
                AddEnemyAnchors(x, y, tilePrefab);
            }



            GameObject actualObject = Instantiate(tilePrefab, new Vector3(x * 2 - 10 + Camera.main.transform.position.x, tilePrefab.transform.position.y + offsetAnimation, y * 2 - 10 + Camera.main.transform.position.z), Quaternion.identity, transform);
            if (actualObject.GetComponent<TibiscuitController>() != null)
            {
                actualObject.transform.parent = null;
            }


            while (Mathf.Abs(Vector3.Distance(actualObject.transform.position, new Vector3(actualObject.transform.position.x, tilePrefab.transform.position.y, actualObject.transform.position.z))) > 0.01f)
            {
                actualObject.transform.position = Vector3.MoveTowards(actualObject.transform.position,
                    new Vector3(actualObject.transform.position.x, tilePrefab.transform.position.y, actualObject.transform.position.z),
                    animationSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

        }
        yield return null; // Wait for the next frame

    }
    public void AddEnemyAnchors(int x, int y, GameObject enemy)
    {
        Vector2 enemyPosition = new Vector2(x * 2 - 10, y * 2 - 10);
        Debug.Log(enemyPosition);
        foreach (EnemyPathData enemyPathData in enemyPathDatas)
        {
            foreach (Vector2 anchor in enemyPathData.anchors)
            {
                if (enemyPosition == anchor)
                {
                    enemy.GetComponent<AgentController>().enemyPathData = enemyPathData;
                }
            }

        }
    }

    public GameObject GetTilePrefab(int tileType)
    {
        foreach (TileData tileData in tileDatas)
        {
            if (tileData.Id == tileType)
            {
                return tileData.prefab;
            }
        }
        return null;
    }

    public void RestartLevel()
    {
        DeleteLevel();
        GenerateLevel();
    }

    public void DeleteLevel()
    {
        foreach (Transform child in transform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void AdvanceToNextLevel()
    {
        Debug.Log("AdvanceToNextLevel");
        Debug.Log(levelDatas.Count + " and " + actualLevelData.Id);
        if (actualLevelData.Id + 1 > levelDatas.Count - 1)
        {
            Debug.LogError("next level doesnt exist");
        }
        else
        {
            actualLevelData = levelDatas[actualLevelData.Id + 1];
            offsetMap = new Vector3(offsetMap.x, offsetMap.y, offsetMap.z + 66);
        }

    }
}
