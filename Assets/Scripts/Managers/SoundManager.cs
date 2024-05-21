using System.Collections.Generic;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    // Manages audio settings such as volume control
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider; // Slider for controlling volume level
        [SerializeField] private List<AudioSource> audioSource; // AudioSource component for playing audio
        [SerializeField] private SoundValue soundValuePrefab; // Prefab for storing and displaying sound value

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Set up initial volume level and attach listener to volume slider
            SetStartVolumeLevel();
            volumeSlider.onValueChanged.AddListener(_ => SetVolumeLevel(volumeSlider.value));
        }

        // Sets the volume level of the audio source and updates the sound value prefab
        private void SetVolumeLevel(float amount)
        {
            // Set the volume of the audio source
            foreach (var sound in audioSource)
            {
                sound.volume = amount / 10;
            }
            // Update the sound value stored in the prefab
            soundValuePrefab.Amount = volumeSlider.value;
        }

        // Sets the initial volume level based on the sound value prefab
        private void SetStartVolumeLevel()
        {
            // Set the volume of the audio source to match the stored sound value
            foreach (var sound in audioSource)
            {
                sound.volume = soundValuePrefab.Amount / 10;
            }
            // Set the initial value of the volume slider to match the stored sound value
            volumeSlider.value = soundValuePrefab.Amount;
        }
    }
}
