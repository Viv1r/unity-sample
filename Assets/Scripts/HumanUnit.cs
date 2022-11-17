using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HumanUnit : MonoBehaviour
{
    public float MaxSpeed = 5;
    public GameObject TinyWoodPrefab;

    internal Tent myTent;

    private GameObject TinyWood;

    private Vector3 InitCoords;
    private Vector3 CurrentTarget;
    private GameObject TargetTree;

    private bool GotWood = false;
    private bool Idle = false;

    private MainController mainController;

    void Start()
    {
        InitCoords = transform.position;
        mainController = FindObjectOfType<MainController>();
        FindTree();
    }

    void Update()
    {
        if (Idle) return;

        float distance = Vector3.Distance(CurrentTarget, transform.position);
        if (distance > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentTarget, MaxSpeed / 20);
        }
        else if (!GotWood)
        {
            CurrentTarget = InitCoords;
            CollectWood();
        }
        else if (GotWood)
        {
            if (myTent)
            {
                DropWood();
                FindTree();
            }
            else
            {
                Idle = true;
            }
        }

        if (TinyWood)
        {
            TinyWood.transform.position = new Vector3
            (
                transform.position.x,
                transform.position.y + (transform.localScale.y / 2) + TinyWood.transform.localScale.y,
                transform.position.z
            );
        }
    }

    void FindTree()
    {
        if (mainController.TreeList.Length == 0)
        {
            Idle = true;
            return;
        }

        int index = Random.Range(0, mainController.TreeList.Length);
        TargetTree = mainController.TreeList[index];
        mainController.RemoveTree(index);
        CurrentTarget = new Vector3
        (
            TargetTree.transform.position.x,
            transform.position.y,
            TargetTree.transform.position.z
        );
    }

    void CollectWood()
    {
        GotWood = true;
        if (TargetTree)
        {
            Destroy(TargetTree);
        }
        if (!TinyWood)
        {
            TinyWood = Instantiate(TinyWoodPrefab);
        }
    }

    void DropWood()
    {
        GotWood = false;
        mainController.AddRes("wood", 5);
        if (TinyWood)
        {
            Destroy(TinyWood);
        }
    }
}
