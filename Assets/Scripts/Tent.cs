using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : MonoBehaviour
{
    public HumanUnit HumanPrefab;
    private HumanUnit Human;

    public AudioSource Emitter;
    public AudioClip FallSound;

    private AudioSource SoundEmitter;

    private float thisX, thisY, thisZ, initSizeX, initSizeY, initSizeZ;
    private bool DestroyMe = false;
    private bool Initialized = false;

    private MainController MainController;

    void Start()
    {
        MainController = FindObjectOfType<MainController>();
        MainController.Buildings["tent"]++;

        thisX = transform.position.x;
        thisY = transform.position.y;
        thisZ = transform.position.z;
        transform.position = new Vector3(thisX, thisY + thisY * 30, thisZ);

        SoundEmitter = Instantiate(Emitter);
        SoundEmitter.clip = FallSound;
        SoundEmitter.Play();
    }

    void Update()
    {
        if (DestroyMe)
        {
            transform.Rotate(0, -5, 0);
            transform.localScale = new Vector3
            (
                transform.localScale.x - initSizeX * 0.01f,
                transform.localScale.y - initSizeY * 0.01f,
                transform.localScale.z - initSizeZ * 0.01f
            );

            if (transform.localScale.y < 0.001)
            {
                Destroy(gameObject);
            }
        }
        else if (transform.position.y > thisY)
        {
            float distance = transform.position.y - thisY;
            float landSpeed = Mathf.Sqrt(distance) / 1.5f;

            if (landSpeed < 1)
            {
                landSpeed = distance;
                Initialized = true;
                SpawnHuman();
            }

            transform.position = new Vector3
            (
                thisX,
                transform.position.y - landSpeed,
                thisZ
            );
        }
    }

    private void SpawnHuman()
    {
        if (Human == null && HumanPrefab != null)
        {
            Human = Instantiate(HumanPrefab);
            Human.transform.position = new Vector3(thisX, thisY - (transform.localScale.y / 2) + (Human.transform.localScale.y / 2), thisZ);
            Human.myTent = this;
        }
    }

    public void DestroyThis()
    {
        if (Initialized && !DestroyMe)
        {
            MainController.Buildings["tent"]--;

            DestroyMe = true;
            initSizeX = transform.localScale.x;
            initSizeY = transform.localScale.y;
            initSizeZ = transform.localScale.z;

            Destroy(SoundEmitter);
        }
    }
}
