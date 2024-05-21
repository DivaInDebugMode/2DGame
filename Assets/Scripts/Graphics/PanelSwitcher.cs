using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Namespace for graphics-related functionality
namespace Graphics
{
    // Class responsible for switching between resolution panels and selecting buttons
    public class ResolutionPanelsSwitcher : MonoBehaviour
    {
        // List of resolution panels
        [SerializeField] private List<GameObject> myPanels = new List<GameObject>();

        // Reference to the EventSystem to manage UI events
        [SerializeField] private EventSystem system;

        // List of selected buttons corresponding to each panel
        [SerializeField] private List<GameObject> selectedButtons = new List<GameObject>();

        // Method to switch between resolution panels
        public void SwitchPanels(int index)
        {
            // Activate/deactivate panels based on the provided index
            myPanels[0].SetActive(index == 0);
            myPanels[1].SetActive(index == 1);
        }

        // Method to set the selected button after a delay
        public void SetSelectButton(int index)
        {
            // Start a coroutine to delay the selection of the button
            switch (index)
            {
                case 0:
                    StartCoroutine(Timer(index));
                    break;
                case 1:
                    StartCoroutine(Timer(index));
                    break;
            }
        }

        // Coroutine to delay the selection of the button
        private IEnumerator Timer(int index)
        {
            // Wait for a short delay
            yield return new WaitForSecondsRealtime(0.05f);
            
            // Set the selected button in the EventSystem
            system.SetSelectedGameObject(selectedButtons[index]);
        }
    }
}