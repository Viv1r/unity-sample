using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeZone : MonoBehaviour
{
    public GameObject[] TreesList;

    private float startX, endX, startZ, endZ, baseY;

    void Start()
    {
        var pos = transform.position;
        var scale = transform.localScale;
        startX = pos.x - (scale.x / 20);
        endX = pos.x + (scale.x / 20);
        startZ = pos.z - (scale.z / 20);
        endZ = pos.z + (scale.z / 20);
        baseY = pos.y - (scale.y / 2);

        GenerateTrees();
    }

    private void GenerateTrees()
    {
        for (float x = startX; x < endX; x += Random.Range(13, 20))
        {
            for (float z = startZ; z < endZ; z += Random.Range(20, 30))
            {
                print("X " + x + ", Z " + z);
                int index = Random.Range(0, TreesList.Length);
                var tree = Instantiate(TreesList[index]);
                tree.transform.position = new Vector3(x, baseY + (tree.transform.localScale.y / 2), z);
            }
        }
    }
}
