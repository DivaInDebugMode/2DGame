using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Panel
{
    // Handles the closing of a secondary panel in the UI
    public class CloseSecondSubPanel : MonoBehaviour
    {
        [SerializeField] private GameObject subPanel; // Reference to the secondary panel
        [SerializeField] private PlayerInput playerInput; // Reference to the player input system
        [SerializeField] private EventSystem eventSystem; // Reference to the event system
        [SerializeField] private GameObject parent; // Reference to the main panel
        [SerializeField] private PanelCondition panelCondition; // Reference to panel condition for UI state tracking

        // Called when the object becomes enabled and active
        private void OnEnable()
        {
            // Subscribe to the "Back" action performed event
            if (playerInput != null) playerInput.actions["Back"].performed += CloseSubPanel;
        }

        // Called when the object becomes disabled or inactive
        private void OnDisable()
        {
            // Unsubscribe from the "Back" action performed event
            if (playerInput != null) playerInput.actions["Back"].performed -= CloseSubPanel;
        }

        // Closes the secondary panel when the "Back" action is triggered
        private void CloseSubPanel(InputAction.CallbackContext context)
        {
            // Check if the secondary panel is active
            if (!subPanel.activeSelf) return;

            // Deactivate the secondary panel
            subPanel.SetActive(false);

            // Update the UI state to indicate no active subpanels
            panelCondition.IsActiveSubPanels = false;

            // If the current selected object is not the main panel, set it as the selected object
            if (eventSystem.currentSelectedGameObject != parent)
            {
                eventSystem.SetSelectedGameObject(parent);
            }
        }
    }
}