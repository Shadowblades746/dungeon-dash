using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    
    public class BossMusic : MonoBehaviour
    {
        public AudioSource intro; // reference to the intro music
        public AudioSource loop; // reference to the loop music

        // Start is called before the first frame update
        void Start()
        {
            intro.Play(); // plays the intro music
            loop.PlayScheduled(AudioSettings.dspTime + intro.clip.length); // plays the loop music after the intro music
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}