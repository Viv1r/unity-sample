using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject SmokePrefab;
    public Material Mat_Main, Mat_Selected;

    private GameObject NewSmoke;
    private float thisX, thisY, thisZ;
    private bool smokeExists = false;

    private bool DestroyMe = false;

    void Start()
    {
        thisX = transform.position.x;
        thisY = transform.position.y;
        thisZ = transform.position.z;
        // NewSmoke = Instantiate(SmokePrefab, new Vector3(thisX, thisY, thisZ), Quaternion.identity);
        transform.position = new Vector3(thisX, thisY-thisY*2.5f, thisZ);

        GetComponent<Renderer>().material.SetColor("Albedo", Color.green);
    }

    void Update()
    {
        if (DestroyMe)
        {
            thisY = transform.position.y;
            transform.position = new Vector3
            (
                thisX,
                thisY + 0.5f,
                thisZ
            );

            if (thisY > 100)
            {
                Destroy(gameObject);
            }
            return;
        }
        if (transform.position.y < thisY)
        {
            transform.position = new Vector3
            (
                thisX,
                transform.position.y + 0.1f,
                thisZ
            );
        }
        else if (smokeExists)
        {
            smokeExists = false;
            Destroy(NewSmoke);
        }
    }

    public void Select()
    {
        GetComponent<Renderer>().material = Mat_Selected;
    }
    
    public void Deselect()
    {
        GetComponent<Renderer>().material = Mat_Main;
    }

    public void Destroy()
    {
        DestroyMe = true;
    }
}
