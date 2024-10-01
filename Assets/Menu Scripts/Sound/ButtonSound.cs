using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sound
{
    // Plays sound when buttons are hovered over
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource; // Reference to the audio source component
        [SerializeField] private AudioClip hoverSound; // Sound clip to play when button is hovered over
        [SerializeField] private List<Button> buttons; // List of buttons to apply hover sound to

        // Called when the object is initialized
        private void Start()
        {
            // Iterate through each button in the list
            foreach (var button in buttons)
            {
                // Get or add an EventTrigger component to the button game object
                var eventTrigger = button.gameObject.GetComponent<EventTrigger>() 
                                   ?? button.gameObject.AddComponent<EventTrigger>();

                // Create an event trigger entry for the select event of the button
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Select
                };
                
                // Add a listener to play hover sound when button is selected
                entry.callback.AddListener((_) => ButtonHoverPlayAudio());
                eventTrigger.triggers.Add(entry);
            }
        }

        // Plays hover sound when a button is hovered over
        private void ButtonHoverPlayAudio()
        {
            // Play the hover sound using the audio source component
            audioSource.PlayOneShot(hoverSound);
        }
    }
}