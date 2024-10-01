using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    // Switches sub panels of options based on button click
    public class OptionsSubPanelsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> myPanel; // List of sub panels
        [SerializeField] private Button parentButton; // Button to activate the sub panel
        [SerializeField] private PanelCondition panelCondition; // Reference to panel condition for UI state tracking

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Add listener to parent button to activate sub panel
            parentButton.onClick.AddListener(() => ActivateMyPanel(0));
        }

        // Activates the specified sub panel
        private void ActivateMyPanel(int index)
        {
            // Activate the specified sub panel
            myPanel[index].SetActive(true);
            
            // Update the UI state to indicate active sub panels
            panelCondition.IsActiveSubPanels = true;
        }
    }
}