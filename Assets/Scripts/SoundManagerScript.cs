using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip scoreSound, deathSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        scoreSound = Resources.Load<AudioClip>("score");
        deathSound = Resources.Load<AudioClip>("death");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string sound)
    {
        switch (sound)
        {
            case "score":
                audioSrc.PlayOneShot(scoreSound);
                break;
            case "death":
                audioSrc.PlayOneShot(deathSound);
                GeneralAudioScript.StopSound();
                break;
        }
    }

}
