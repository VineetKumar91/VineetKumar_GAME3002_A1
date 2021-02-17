using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio sources for scored, missed and ending
    public AudioSource Audio_Scored;
    public AudioSource Audio_Missed;
    public AudioSource Audio_Ending;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Individual function to play those Audio Sources
    public void PlayScored()
    {
        Audio_Scored.Play();
    }

    public void PlayMissed()
    {
        Audio_Missed.Play();
    }

    public void PlayEnding()
    {
        Audio_Ending.Play();
    }
}
