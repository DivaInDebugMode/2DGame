using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Panel
{
    // Switches between option panels based on button selection
    public class OptionsMainPanelsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> panels; // List of option panels
        [SerializeField] private int activeIndex = -1; // Index of the currently active option panel
        [SerializeField] private List<Button> optionButtons = new List<Button>(); // List of buttons corresponding to option panels
        [SerializeField] private int number; // Number of valid option panels
        [SerializeField] private SceneIndex sceneIndex;

        // Property to access the active index
        public int ActiveIndex
        {
            get => activeIndex;
            set => activeIndex = value;
        }
   
        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Iterate through option buttons to set up event triggers for each
            for (var i = 0; i < optionButtons.Count; i++)
            {
                var index = i;
                var eventTrigger = optionButtons[i].gameObject.GetComponent<EventTrigger>() 
                                   ?? optionButtons[i].gameObject.AddComponent<EventTrigger>();
          
                // Create an event trigger for the select event of the button
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Select
                };
                // Add a listener to switch option panels when the button is selected
                entry.callback.AddListener((_) => SwitchOptionPanels(index));
                eventTrigger.triggers.Add(entry);
            }
        }

        private void OnEnable()
        {
            if(sceneIndex.Index < 1) return;
            SwitchOptionPanels(0);
        }

        // Switches between option panels based on the selected button index
        private void SwitchOptionPanels(int index)
        {
            // Deactivate the currently active panel if there is one
            if (activeIndex != -1)
                panels[activeIndex].SetActive(false);

            // Update the active index based on the selected button index
            if (index <= number)
                activeIndex = index;
            else
                activeIndex = -1;

            // Activate the newly selected panel if there is one
            if (activeIndex != -1)
            {
                panels[activeIndex].SetActive(true);
            }
        }
    }
}