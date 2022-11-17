using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public AudioSource Emitter;
    public AudioClip Woodcut;
    public AudioClip[] LaughList, ScreamList;

    private MainController MainController;

    private bool invoked = false;

    void Start()
    {
        MainController = FindObjectOfType<MainController>();
    }

    void Update()
    {
        if (!invoked && MainController.Buildings["tent"] >= 1)
        {
            invoked = true;
            InvokeRepeating("playSound", 5, 10);
        }
        else if (invoked && MainController.Buildings["tent"] <= 0)
        {
            invoked = false;
            CancelInvoke("playSound");
        }
    }

    public void playSound()
    {
        if (LaughList.Length > 0)
        {
            var tempEmitter = Instantiate(Emitter);
            tempEmitter.clip = Woodcut;
            tempEmitter.Play();
        }
        int chance = Random.Range(0, 2);
        if (chance == 0)
        {
            var tempEmitter = Instantiate(Emitter);
            var sounds = new List<AudioClip>();
            sounds.AddRange(LaughList);
            sounds.AddRange(ScreamList);
            
            int index = Random.Range(0, sounds.Count);
            tempEmitter.clip = sounds[index];
            tempEmitter.PlayDelayed(5);
        }
    }
}
