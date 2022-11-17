using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public AudioSource Emitter;
    public AudioClip PlaceSound;

    void Start()
    {
        var SoundEmitter = Instantiate(Emitter);
        SoundEmitter.clip = PlaceSound;
        SoundEmitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
