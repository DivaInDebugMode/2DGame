using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    // Manages VSync settings and frame rate limit
    public class VsyncManager : MonoBehaviour
    {
        [SerializeField] private Toggle vSyncToggle; // Toggle for enabling/disabling VSync
        [SerializeField] private Button vSyncButton; // Button for toggling VSync

        private const int FrameRateLimit = 90; // Frame rate limit

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Set the target frame rate to the specified limit
            Application.targetFrameRate = FrameRateLimit;
            
            // Add a listener to the VSync toggle to handle value changes
            vSyncToggle.onValueChanged.AddListener(_ => SetVSync());
            
            // Get or add EventTrigger component to button game object
            var eventTrigger = vSyncButton.gameObject.GetComponent<EventTrigger>()
                               ?? vSyncButton.gameObject.AddComponent<EventTrigger>();

            // Create a new EventTrigger entry for button submit event
            var entry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.Submit
            };
            // Add a listener to toggle VSync when button is submitted
            entry.callback.AddListener(_ => VSyncToggleCondition());
            eventTrigger.triggers.Add(entry);

            // Set the initial state of the VSync toggle based on current QualitySettings.vSyncCount
            vSyncToggle.isOn = QualitySettings.vSyncCount != 0;
        }

        // Sets the VSync count based on the state of the VSync toggle
        private void SetVSync()
        {
            QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
        }

        // Toggles the condition of the VSync toggle (used as button click event)
        private void VSyncToggleCondition()
        {
            vSyncToggle.isOn = !vSyncToggle.isOn;
        }
    }
}