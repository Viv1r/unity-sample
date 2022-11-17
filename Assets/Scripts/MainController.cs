using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public Text IronBar, WoodBar, FoodBar, TentBar;

    public float WorldWidth = 400, WorldHeight = 320;

    internal GameObject[] TreeList;
    internal string buildingType;

    public Dictionary<string, int> Resources = new()
    {
        { "iron", 2500 },
        { "wood", 500 },
        { "food", 100 }
    };

    public Dictionary<string, int> Prices = new()
    {
        { "tent", 50 },
        { "shack", 200 },
        { "road", 30 }
    };

    public Dictionary<string, int> Buildings = new()
    {
        { "tent", 0 },
        { "shack", 0 }
    };

    private void Start()
    {
        TreeList = GameObject.FindGameObjectsWithTag("Tree");
    }

    private void Update()
    {
        updateBuildings();
    }

    private void updateRes()
    {
        IronBar.text = "" + Resources["iron"];
        WoodBar.text = "" + Resources["wood"];
        FoodBar.text = "" + Resources["food"];
    }

    private void updateBuildings()
    {
        TentBar.text = "" + Buildings["tent"];
    }

    public void SetRes(string name, int value)
    {
        if (!Resources.ContainsKey(name))
            return;
        Resources[name] = value;
        updateRes();
    }

    public void AddRes(string name, int value)
    {
        if (!Resources.ContainsKey(name))
            return;
        Resources[name] += value;
        updateRes();
    }

    public int GetRes(string name)
    {
        if (!Resources.ContainsKey(name))
            return 0;
        return Resources[name];
    }

    public void RemoveTree(int index)
    {
        TreeList[index] = null;
        TreeList = TreeList.Where(elem => elem != null).ToArray();
    }
}
