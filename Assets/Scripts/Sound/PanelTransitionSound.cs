using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public class PanelTransitionSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        public void PanelSound()
        {
            // Play the panel transition sound using the audio source component
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}