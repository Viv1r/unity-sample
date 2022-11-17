using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YouSpinMeRightRound : MonoBehaviour
{
    public bool Clockwise = false;
    public bool SetBuilding = false;
    public string BuildingName = "";

    private Vector3 initScale;
    
    void Start()
    {
        initScale = transform.localScale;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3
        (
            transform.eulerAngles.x,
            transform.eulerAngles.y + (Clockwise ? 2 : -2),
            transform.eulerAngles.z
        );
    }

    private void OnMouseEnter()
    {
        float scaleMultiplier = 1.1f;
        transform.localScale = new Vector3
        (
            initScale.x * scaleMultiplier,
            initScale.y * scaleMultiplier,
            initScale.z * scaleMultiplier
        );
    }

    private void OnMouseExit()
    {
        transform.localScale = initScale;
    }

    private void OnMouseDown()
    {
        if (SetBuilding)
        {
            var mainController = FindObjectOfType<MainController>();
            mainController.buildingType = BuildingName.ToLower();
        }
    }
}
