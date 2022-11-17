using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GridTile : MonoBehaviour
{
    public Material Mat_Main, Mat_Selected;
    public GameObject BuildingPrefab;
    public GameObject BuildingPreviewPrefab;
    public GameObject Road_Straight;
    public GameObject Road_Cross;
    public GameObject Road_Triple;
    public GameObject Road_Turn;
    public GameObject Road_End;

    private MainController mainController;
    private GameObject newBuilding, buildingPreview;
    private Renderer objectRenderer;

    internal MainGrid MyGrid;
    internal int thisI = 0, thisJ = 0;
    
    private float thisX, thisY, thisZ;

    private Vector3 mouseStartPos;

    public GridTile(Material mat_Main, Material mat_Selected)
    {
        Mat_Main = mat_Main;
        Mat_Selected = mat_Selected;
    }

    void Start()
    {
        mainController = FindObjectOfType<MainController>();

        objectRenderer = GetComponent<Renderer>();
        SetMaterial(Mat_Main);

        thisX = transform.position.x;
        thisY = transform.position.y;
        thisZ = transform.position.z;

        MyGrid = FindObjectOfType<MainGrid>();
    }

    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        SetMaterial(Mat_Selected);
        if (!newBuilding && !buildingPreview && mainController.buildingType == "tent")
        {
            createBuildingPreview();
        }
    }

    private void OnMouseExit()
    {
        SetMaterial(Mat_Main);
        if (buildingPreview)
        {
            Destroy(buildingPreview);
        }
    }

    private void OnMouseDown()
    {
        mouseStartPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (Vector3.Distance(mouseStartPos, Input.mousePosition) > 1) return;

        if (mainController.buildingType == "tent")
        {
            if (newBuilding)
            {
                newBuilding.GetComponent<Tent>().DestroyThis();
                mainController.AddRes("wood", mainController.Prices["tent"] / 2);
            }
            else if (mainController.GetRes("wood") >= mainController.Prices["tent"])
            {
                mainController.AddRes("wood", -mainController.Prices["tent"]);
                createBuilding();
            }
        }
        if (mainController.buildingType == "road")
        {
            if (newBuilding)
            {
                Destroy(newBuilding);
                mainController.AddRes("iron", mainController.Prices["road"] / 2);
                MyGrid.setRoad(thisI, thisJ, false);
            }
            else if (mainController.GetRes("iron") >= mainController.Prices["road"])
            {
                mainController.AddRes("iron", -mainController.Prices["road"]);
                createRoad("cross", 0);
                MyGrid.setRoad(thisI, thisJ, true);
            }
        }
    }

    private void createBuilding()
    {
        float targetX = objectRenderer.bounds.size.x * 0.6f;
        float targetZ = objectRenderer.bounds.size.x * 0.6f;
        float targetY = (targetX + targetZ) / 2;

        newBuilding = Instantiate(BuildingPrefab, new Vector3(thisX, thisY + targetY / 2, thisZ), Quaternion.identity);
        newBuilding.transform.eulerAngles = new Vector3(0, Random.Range(0, 181), 0);
        properSize(newBuilding, targetX, targetY, targetZ);

        if (buildingPreview)
        {
            Destroy(buildingPreview);
        }
    }

    private void createBuildingPreview()
    {
        float targetX = objectRenderer.bounds.size.x * 0.6f;
        float targetZ = objectRenderer.bounds.size.x * 0.6f;
        float targetY = (targetX + targetZ) / 2;

        buildingPreview = Instantiate(BuildingPreviewPrefab, new Vector3(thisX, thisY + targetY / 2, thisZ), Quaternion.identity);
        buildingPreview.transform.eulerAngles = new Vector3(0, 90, 0);
        properSize(buildingPreview, targetX, targetY, targetZ);
    }

    private void properSize(GameObject tent, float targetX, float targetY, float targetZ)
    {
        tent.transform.localScale = new Vector3
        (
            targetX,
            targetY,
            targetZ
        );
        tent.transform.parent = transform;
    }

    public void SetMaterial(Material mat)
    {
        objectRenderer.material = mat;
    }

    public void createRoad(string roadType, int rotY)
    {
        if (newBuilding)
        {
            Destroy(newBuilding);
        }

        newBuilding = Instantiate(getRoadPrefab(roadType));

        properSize(newBuilding, objectRenderer.bounds.size.x, objectRenderer.bounds.size.x / 8, objectRenderer.bounds.size.z);

        newBuilding.transform.position = new Vector3
        (
            thisX,
            thisY + transform.localScale.y / 2 + newBuilding.transform.localScale.y / 2,
            thisZ
        );

        newBuilding.transform.eulerAngles = new Vector3(0, rotY, 0);
    }

    private GameObject getRoadPrefab(string roadType)
    {
        switch (roadType)
        {
            case "straight": return Road_Straight;
            case "triple": return Road_Triple;
            case "turn": return Road_Turn;
            case "end": return Road_End;
            default: return Road_Cross;
        }
    }
}
