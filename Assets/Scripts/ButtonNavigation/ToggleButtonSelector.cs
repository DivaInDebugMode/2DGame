using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Namespace for the button navigation functionality
namespace ButtonNavigation
{
    // Class responsible for setting the selected button when a toggle is clicked
    public class ToggleButtonSelector : MonoBehaviour
    {
        // List of toggles to monitor for clicks
        [SerializeField] private List<Toggle> toggles = new List<Toggle>();
        
        // List of buttons corresponding to the toggles
        [SerializeField] private List<Button> buttons = new List<Button>();
        
        // Reference to the EventSystem to manage UI events
        [SerializeField] private EventSystem system;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Loop through all toggles
            for (var i = 0; i < toggles.Count; i++)
            {
                // Store the current index to avoid closure issues
                var toggleIndex = i;

                // Add EventTrigger component to toggle game object to handle events
                var trigger = toggles[i].gameObject.AddComponent<EventTrigger>();

                // Create a new event entry for PointerClick event
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };

                // Add a listener to the PointerClick event to set the selected button
                entry.callback.AddListener(_ => SetCurrentSelectedButton(toggleIndex));
                trigger.triggers.Add(entry);
            }
        }

        // Method to set the selected button when a toggle is clicked
        private void SetCurrentSelectedButton(int index)
        {
            // Set the corresponding button as the selected object
            system.SetSelectedGameObject(buttons[index].gameObject);
        }
    }
}
