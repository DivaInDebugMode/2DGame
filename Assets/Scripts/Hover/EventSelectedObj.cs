using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Namespace for hover-related functionality
namespace Hover
{
    // Class responsible for handling selected objects in UI events
    public class EventSelectedObj : MonoBehaviour
    {
        // Reference to the PauseCondition script
        [SerializeField] private PauseCondition pauseCondition;

        // Reference to the settings button
        [SerializeField] private Button settingsButton;

        // Reference to the start game button
        [SerializeField] private Button startGameButton;

        // Reference to the EventSystem to manage UI events
        [SerializeField] private EventSystem eventSystem;

        // Reference to the OptionButtonHover script for start button
        [SerializeField] private OptionButtonHover startHover;

        // Reference to the OptionButtonHover script for option button
        [SerializeField] private OptionButtonHover optionHover;

        // Reference to the back button
        [SerializeField] private Button backButton;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Depending on the value of isOptionPressed, set the selected button and adjust UI accordingly
            switch (pauseCondition.isOptionPressed)
            {
                case true:
                    // Set the selected game object to the settings button
                    eventSystem.SetSelectedGameObject(settingsButton.gameObject);

                    // Set the active index of the option hover script to 0
                    optionHover.ActiveIndex = 0;

                    // Change the color of the settings button to white to indicate selection
                    settingsButton.targetGraphic.color = Color.white;
                    break;
                case false:
                    // Set the selected game object to the start game button
                    eventSystem.SetSelectedGameObject(startGameButton.gameObject);

                    // Set the active index of the start hover script to 0
                    startHover.ActiveIndex = 0;

                    // Change the color of the start game button to white to indicate selection
                    startGameButton.targetGraphic.color = Color.white;
                    break;
            }
            
            // Add a listener to the back button to update the condition when clicked
            backButton.onClick.AddListener(SetCondition);
        }

        // Method to set the condition when the back button is clicked
        private void SetCondition()
        {
            // Set isOptionPressed to false when the back button is clicked
            pauseCondition.isOptionPressed = false;
        }
    }
}