using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
public class LevelGen : MonoBehaviour
{

    public List<LevelData> levelDatas;
    public List<TileData> tileDatas;
    public List<EnemyPathData> enemyPathDatas;
    public LevelData actualLevelData;

    public GameObject Ground;
    public NavMeshSurface surface;
    public int width = 11;
    public int height = 11;



    // Use this for initialization
    void Start()
    {
        actualLevelData = levelDatas[0];
        Instantiate(Ground);
        GenerateLevel();
        

    }



    // Create a grid based level
    void GenerateLevel()
    {
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
                        CreateTile(i, y, arrayInt[i][y]);
                    }

                }
            }
        }

    }
    public void CreateTile(int x, int y, int tileType)
    {
        GameObject tilePrefab = GetTilePrefab(tileType);
        if (tilePrefab.GetComponent<AgentController>())
        {
            Debug.Log("enemy");
            AddEnemyAnchors(x, y, tilePrefab);
        }
        if (tilePrefab != null)
        {
            Instantiate(tilePrefab, new Vector3(x * 2 - 10, tilePrefab.transform.position.y, y * 2 - 10), Quaternion.identity);
        }


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

}
