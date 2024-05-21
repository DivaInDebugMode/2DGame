using System;
using System.Collections;
using System.Collections.Generic;
using Panel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hover
{
    public class OptionsAction : MonoBehaviour
    {
        // Serialized fields can be set in the Unity Inspector
        [SerializeField] private PanelCondition panelCondition; // Condition to check if sub panels are active
        [SerializeField] private PlayerInput characterInput; // Player input actions
        [SerializeField] private EventSystem eventSystem; // Reference to the Event System
        [SerializeField] private OptionButtonHover optionButtonHover; // Script handling hover for buttons
        [SerializeField] private List<Button> mainOptionButton = new(); // List of main option buttons
        [SerializeField] private List<Button> sliderButtons = new(); // List of slider buttons
        [SerializeField] private List<Slider> sliders = new(); // List of sliders
        [SerializeField] private List<OptionButtonHover> optionButtonHovers = new(); // List of scripts handling hover for buttons
        [SerializeField] private GameObject questionPanel;
        [SerializeField] private GameObject optionsPanel;

        private void Awake()
        {
            foreach (var button in mainOptionButton)
            {
                // Add an EventTrigger component to the button if not already present
                var eventTrigger = button.gameObject.GetComponent<EventTrigger>() 
                                   ?? button.gameObject.AddComponent<EventTrigger>();

                // Create a new event entry for the Select event
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Select
                };

                // Add a listener to the Select event to handle button properties
                entry.callback.AddListener((_) => SetMainButtonsActiveIndex());
                eventTrigger.triggers.Add(entry);
            }
        }

        // Called when the GameObject becomes enabled and active
        private void OnEnable()
        {
            // Subscribe to the Back action performed event
            characterInput.actions["Back"].performed += DeselectWithAction;
        }

        // Called when the GameObject becomes disabled or inactive
        private void OnDisable()
        {
            // Unsubscribe from the Back action performed event
            characterInput.actions["Back"].performed -= DeselectWithAction;
        }

        // Method to handle deselection when the Back action is performed
        private void DeselectWithAction(InputAction.CallbackContext context)
        {
            // If sub panels are active, return
            if (panelCondition.IsActiveSubPanels) return;
            // If the currently selected GameObject is a slider or its associated button, change their color
            if (eventSystem.currentSelectedGameObject == sliderButtons[0].gameObject
                || eventSystem.currentSelectedGameObject == sliderButtons[1].gameObject ||
                eventSystem.currentSelectedGameObject == sliders[0].gameObject ||
                eventSystem.currentSelectedGameObject == sliders[1].gameObject)
            {
                foreach (var button in sliderButtons)
                    button.targetGraphic.color = Color.grey;
            }

            // Depending on the active index of optionButtonHover, perform different actions
            if (questionPanel.activeSelf || !optionsPanel.activeSelf) return;
            switch (optionButtonHover.ActiveIndex)
            {
                case 0:
                case 1:
                case 2:
                    // Start a coroutine to delay selection change
                    StartCoroutine(Timer());
                    // Set the selected GameObject to the corresponding main option button
                    eventSystem.SetSelectedGameObject(mainOptionButton[optionButtonHover.ActiveIndex].gameObject);
                    break;
            }
        }
        
        // Coroutine to reset hover indexes after a delay
        private IEnumerator Timer()
        {
            // Wait for a short duration
            yield return new WaitForSecondsRealtime(0.05f); 
            // Reset the active index for all OptionButtonHover scripts
            foreach (var t in optionButtonHovers) t.ActiveIndex = -1;
        }

        private void SetMainButtonsActiveIndex()
        {
            StartCoroutine(Timer());
        }
    }
}