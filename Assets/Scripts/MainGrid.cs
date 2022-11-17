using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MainGrid : MonoBehaviour
{
    public float horizontalTiles = 8, verticalTiles = 8;
    public Material Mat_Main, Mat_Selected;
    public GameObject BuildingPrefab;
    public GameObject TilePrefab;

    private List<List<bool>> RoadGrid = new();

    private static List<List<GameObject>> TileList = new();

    private float LevelY, tileWidth, tileHeight;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.enabled = false;
        LevelY = transform.position.y;
        tileWidth = objectRenderer.bounds.size.x / horizontalTiles;
        tileHeight = objectRenderer.bounds.size.z / verticalTiles;
        FillRoads();
        GenerateGrid();
    }

    void Update()
    {

    }

    private void GenerateGrid()
    {
        var pos = transform.position;
        float startX = pos.x - tileWidth * (horizontalTiles / 2) + (tileWidth / 2);
        float endX = pos.x + tileWidth * (horizontalTiles / 2) + (tileWidth / 2);
        float startZ = pos.z - tileHeight * (verticalTiles / 2) + (tileHeight / 2);
        float endZ = pos.z + tileHeight * (verticalTiles / 2) + (tileHeight / 2);

        for (float x = startX; x < endX; x += tileWidth)
        {
            TileList.Add(new List<GameObject>());
            for (float z = startZ; z < endZ; z += tileHeight)
            {
                GameObject newTile = Instantiate(TilePrefab, new Vector3(x, LevelY, z), Quaternion.identity) as GameObject;
                newTile.transform.localScale = new Vector3(tileWidth / 10, 1, tileHeight / 10);
                newTile.transform.parent = transform;

                TileList.Last().Add(newTile);
                newTile.GetComponent<GridTile>().thisI = TileList.Count - 1;
                newTile.GetComponent<GridTile>().thisJ = TileList.Last().Count - 1;
            }
        }
    }

    private static Dictionary<string, List<string>> roadsClarificationType = new()
    {
        {
            "straight", new() { "True False True False", "False True False True" }
        },
        {
            "turn", new() { "True True False False", "False False True True", "True False False True", "False True True False" }
        },
        {
            "triple", new() { "False True True True", "True False True True", "True True False True", "True True True False" }
        },
        {
            "cross", new() { "True True True True", "False False False False" }
        },
        {
            "end", new() { "True False False False", "False True False False", "False False True False", "False False False True" }
        },
    };

    private static Dictionary<int, List<string>> roadsClarificationRot = new()
    {
        {
            0, new() { "False False False False", "True True True True", "False True False True", "False True True True", "True True False False", "False True False False" }
        },
        {
            90, new() { "True False True False", "False False True False", "True False True True", "False True True False" }
        },
        {
            180, new() { "True True False True", "False False True True", "False False False True" } 
        },
        {
            270, new() { "True False False False", "True True True False", "True False False True" }
        }
    };

    public void setRoad(int index_i, int index_j, bool state)
    {
        RoadGrid[index_i][index_j] = state;
        CheckRoads();
    }

    private void FillRoads()
    {
        for (int i = 0; i < verticalTiles; i++)
        {
            RoadGrid.Add(new List<bool>());
            for(int j = 0; j < horizontalTiles; j++)
            {
                RoadGrid[i].Add(false);
            }
        }
    }

    private void CheckRoads()
    {
        for (int i = 0; i < RoadGrid.Count; i++)
        {
            for (int j = 0; j < RoadGrid[i].Count; j++)
            {
                if (RoadGrid[i][j] == true)
                {
                    List<bool> surround = new();

                    try { surround.Add(RoadGrid[i-1][j]); } catch { surround.Add(false); }
                    try { surround.Add(RoadGrid[i][j+1]); } catch { surround.Add(false); }
                    try { surround.Add(RoadGrid[i+1][j]); } catch { surround.Add(false); }
                    try { surround.Add(RoadGrid[i][j-1]); } catch { surround.Add(false); }

                    GameObject targetTile = TileList[i][j];

                    string query = string.Join(" ", surround);
                    string roadType = "cross";

                    var clarifyType = roadsClarificationType.FirstOrDefault(elem => elem.Value.Contains(query));
                    int clarifyRot = roadsClarificationRot.FirstOrDefault(elem => elem.Value.Contains(query)).Key;

                    if (clarifyType.Key != null)
                    {
                        roadType = clarifyType.Key;
                    }

                    targetTile.GetComponent<GridTile>().createRoad(roadType, clarifyRot);
                }
            }
        }
    }
}
