using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAudioScript : MonoBehaviour
{

    public static AudioClip generalSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        generalSound = Resources.Load<AudioClip>("general");

        audioSrc = GetComponent<AudioSource>();

        PlaySound("general");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string sound)
    {
        switch (sound)
        {
            case "general":
                audioSrc.PlayOneShot(generalSound);
                break;
        }
    }

    public static void StopSound(string sound)
    {
        switch (sound)
        {
            case "general":
                audioSrc.Stop();
                break;
        }
    }
}
