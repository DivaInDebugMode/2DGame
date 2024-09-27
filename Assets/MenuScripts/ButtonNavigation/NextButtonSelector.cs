using UnityEngine;
using UnityEngine.EventSystems;

// Namespace for the button navigation functionality
namespace ButtonNavigation
{
    // Class responsible for selecting the next object upon submission
    public class NextButtonSelector : MonoBehaviour
    {
        // Reference to the EventSystem to manage UI events
        [SerializeField] private EventSystem system;
        
        // Reference to the next object to select
        [SerializeField] private GameObject nextObj;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Get or add EventTrigger component to handle events
            var eventTrigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

            // Create a new event entry for Submit event
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Submit
            };

            // Add a listener to the Submit event to call SelectNextObj method
            entry.callback.AddListener((_) => SelectNextObj());
            eventTrigger.triggers.Add(entry);
        }

        // Method to select the next object when the current object is selected
        private void SelectNextObj()
        {
            // Check if the current selected object is this object
            if (system.currentSelectedGameObject == gameObject)
            {
                // Set the next object as the selected object
                system.SetSelectedGameObject(nextObj);
            }
        }
    }
}