using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    // Manages screen settings such as fullscreen toggle and button interactions
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private Toggle fullScreenToggle; // Toggle for controlling fullscreen mode
        [SerializeField] private Button button; // Button for triggering screen mode toggle

        // Called when the object becomes enabled and active
        private void Awake()
        { 
            // Set initial state of fullscreen toggle based on current screen mode
            fullScreenToggle.isOn = Screen.fullScreen;
            
            // Get or add EventTrigger component to button game object
            var eventTrigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        
            // Create a new EventTrigger entry for button submit event
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Submit
            };
            // Add a listener to toggle screen mode when button is submitted
            entry.callback.AddListener(_ => ToggleCondition());
            eventTrigger.triggers.Add(entry);
            
            // Add a listener to apply graphics settings when fullscreen toggle value changes
            fullScreenToggle.onValueChanged.AddListener(_ => ApplyGraphics());
        }
    
        // Apply the graphics settings based on the state of the fullscreen toggle
        private void ApplyGraphics()
        {
            Screen.fullScreen = fullScreenToggle.isOn;
        }

        // Toggle the condition of the fullscreen toggle (used as button click event)
        private void ToggleCondition()
        { 
            fullScreenToggle.isOn = !fullScreenToggle.isOn;
        }
    }
}
