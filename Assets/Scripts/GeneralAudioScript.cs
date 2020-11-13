using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAudioScript : MonoBehaviour
{

    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = true;
        PlaySound();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound()
    {
       audioSrc.Play();
    }

    public static void StopSound()
    {
        audioSrc.Stop();
    }
}
