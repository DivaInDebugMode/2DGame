using System.Collections.Generic;
using Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Panel
{
    // Manages switching between input panels
    public class InputPanelsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> inputPanel = new(); // List of input panels
        [SerializeField] private EventSystem system; // Reference to the EventSystem
        [SerializeField] private List<Button> button; // List of buttons associated with input panels
        [SerializeField] private PlayerInput playerInput; // Reference to the PlayerInput
        [SerializeField] private PanelCondition panelCondition; // Reference to the panel condition for UI state tracking
        [SerializeField] private RebindCondition rebindCondition; // Reference to the rebind condition for rebind state tracking
        
        // Index of the last selected button
        private const int LastSelectedButtonIndex = 0;

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Iterate through buttons to set up event triggers for each
            for (var i = 0; i < button.Count; i++)
            {
                var index = i;
                var eventTrigger = button[i].gameObject.GetComponent<EventTrigger>()
                                   ?? button[i].gameObject.AddComponent<EventTrigger>();

                // Create an event trigger for the select event of the button
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Select
                };
                // Add a listener to close the corresponding input panel when the button is selected
                entry.callback.AddListener((_) => ClosePanel(index));
                eventTrigger.triggers.Add(entry);
            }
        }

        // Called when the object becomes enabled and active
        private void OnEnable()
        {
            // Subscribe to the "Back" action
            if (playerInput != null) 
                playerInput.actions["Back"].performed += CloseAllPanels;
        }

        // Called when the object becomes disabled or inactive
        private void OnDisable()
        {
            // Unsubscribe from the "Back" action
            if (playerInput != null) 
                playerInput.actions["Back"].performed -= CloseAllPanels;
        }
        
        // Closes the specified input panel
        private void ClosePanel(int index)
        {
            // Check if the current selected object is the associated button and if the panel is active
            if (system.currentSelectedGameObject == button[index].gameObject && inputPanel[index].activeSelf)
            {
                // Deactivate the input panel and update the UI state
                inputPanel[index].SetActive(false);
                panelCondition.IsActiveSubPanels = false;
            }
        }

        // Closes all input panels
        private void CloseAllPanels(InputAction.CallbackContext context)
        {
            // Check if the first input panel is active and not in rebind state
            if (inputPanel[0].activeSelf && !rebindCondition.IsRebinding)
            {
                // Iterate through input panels to deactivate them and update the UI state
                foreach (var panel in inputPanel)
                {
                    panel.SetActive(false);
                    panelCondition.IsActiveSubPanels = false;
                }
                // Set the last selected button as the selected object
                system.SetSelectedGameObject(button[LastSelectedButtonIndex].gameObject);
            }
            // Check if the second input panel is active, not in rebind state, and there are more than one input panels
            else if (inputPanel.Count > 1 && inputPanel[1].activeSelf && !rebindCondition.IsRebinding)
            {
                // Iterate through input panels to deactivate them and update the UI state
                foreach (var panel in inputPanel)
                {
                    panel.SetActive(false);
                    panelCondition.IsActiveSubPanels = false;
                }
                // Set the second button as the selected object
                system.SetSelectedGameObject(button[1].gameObject);
            }
        }
    }
}