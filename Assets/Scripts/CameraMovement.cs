using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool moveCamera = false;
    private bool rotateCamera = false;
    private float cameraZoom = 3;
    private float startY = 0;

    private const float minZoom = 0, maxZoom = 7;
    private const float minRot = 30, maxRot = 60;

    private MainController mainController;

    private Vector3 Move_MouseStartPos, Move_CameraStartPos;
    private Vector3 Rotate_MouseStartPos, Rotate_CameraStartPos;

    void Start()
    {
        mainController = FindObjectOfType<MainController>();
        startY = transform.position.y;
        correctPos();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move_MouseStartPos = Input.mousePosition;
            Move_CameraStartPos = transform.position;
            moveCamera = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            moveCamera = false;
        }

        if (moveCamera)
        {
            float zoomMod = (float)Mathf.Sqrt((cameraZoom + 1) * 5);
            float targetX = Move_CameraStartPos.x - (Input.mousePosition.x - Move_MouseStartPos.x) / 40f * zoomMod;
            float targetZ = Move_CameraStartPos.z - (Input.mousePosition.y - Move_MouseStartPos.y) / 40f * zoomMod;

            float maxX = mainController.WorldWidth / 2;
            float maxZ = mainController.WorldHeight / 2;

            if (targetX > maxX || targetX < -maxX)
            {
                targetX = transform.position.x;
            }
            if (targetZ > maxZ || targetZ < -maxZ)
            {
                targetZ = transform.position.z;
            }

            transform.position = new Vector3
            (
                targetX,
                transform.position.y,
                targetZ
            );
        }

        if (Input.GetMouseButtonDown(1))
        {
            Rotate_MouseStartPos = Input.mousePosition;
            Rotate_CameraStartPos = new Vector3
			(
				transform.localRotation.eulerAngles.x,
				transform.localRotation.eulerAngles.y,
				transform.localRotation.eulerAngles.z
			);
			
            rotateCamera = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rotateCamera = false;
        }

        if (rotateCamera)
        {
            transform.eulerAngles = new Vector3
            (
                Rotate_CameraStartPos.x,
                Rotate_CameraStartPos.y - (Rotate_MouseStartPos.x - Input.mousePosition.x) / 2.5f,
                Rotate_CameraStartPos.z
            );
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0) return;

        if (scroll < 0 && cameraZoom < maxZoom)
            cameraZoom++;
        else if (scroll > 0 && cameraZoom > minZoom)
            cameraZoom--;

        correctPos();
    }

    private void correctPos()
    {
        float newPosY = startY + (cameraZoom - 3) * 6;
        float newRotX = minRot + cameraZoom * ((maxRot - minRot) / maxZoom);

        if (newRotX > 90) newRotX = 90;
        if (newRotX < 0) newRotX = 0;

        transform.position = new Vector3(
            transform.position.x,
            newPosY,
            transform.position.z
        );

        transform.eulerAngles = new Vector3
        (
            newRotX,
            transform.localRotation.eulerAngles.y,
            transform.localRotation.eulerAngles.z
        );
    }
}
