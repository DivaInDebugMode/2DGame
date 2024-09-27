using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Namespace for hover-related functionality
namespace Hover
{
    // Class responsible for handling hover effects on option buttons
    public class OptionButtonHover : MonoBehaviour
    {
        // List of option buttons
        [SerializeField] private List<Button> optionButton = new List<Button>();

        // Index of the currently active button
        [SerializeField] private int activeIndex = -1;

        // Property to get and set the active index
        public int ActiveIndex
        {
            get => activeIndex;
            set => activeIndex = value;
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Iterate through all option buttons
            for (var i = 0; i < optionButton.Count; i++)
            {
                // Store the current index in a local variable to avoid closure issues
                var index = i;

                // Add an EventTrigger component to the button if not already present
                var eventTrigger = optionButton[i].gameObject.GetComponent<EventTrigger>() 
                                   ?? optionButton[i].gameObject.AddComponent<EventTrigger>();

                // Create a new event entry for the Select event
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Select
                };

                // Add a listener to the Select event to handle button properties
                entry.callback.AddListener((_) => ButtonProperties(index));
                eventTrigger.triggers.Add(entry);
            }
        }

        // Method to handle button properties when selected
        private void ButtonProperties(int index)
        {
            // Reset the color of the previously active button to grey if there was one
            if (activeIndex != -1)
            {
                optionButton[activeIndex].targetGraphic.color = Color.grey;
            }
            
            // Update the active index to the newly selected button
            activeIndex = index;
            
            // Change the color of the selected button to white to indicate selection
            optionButton[index].targetGraphic.color = Color.white;
        }
    }
}