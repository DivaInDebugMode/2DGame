using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Namespace for graphics-related functionality
namespace Graphics
{
    // Class responsible for setting graphics quality
    public class SetGraphics : MonoBehaviour
    {
        // List of buttons to select different graphics settings
        [SerializeField] private List<Button> graphicsButtons = new();
        // TextMeshProUGUI component to display quality level
        [SerializeField] private TextMeshProUGUI qualityText;
        [SerializeField] private List<TextMeshProUGUI> qualityTexts = new();
        // Reference to the GraphicsManager for managing graphics settings
        [SerializeField] private GraphicsManager graphicsManager;

        private bool canTextChange;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Update the displayed quality level text
            SetQualityText();
            // Add listeners to the graphics buttons
            for (var i = 0; i < graphicsButtons.Count; i++)
            {
                // Capture the current button index in a local variable to avoid closure issues
                var number = i;

                // Add a listener to each button to set the graphics quality
                graphicsButtons[i].onClick.AddListener(() => SetGraphicsQuality(number));
            }
        }

        private void OnEnable()
        {
            if (!canTextChange) return;
            SetQualityText();
            canTextChange = false;
        }

        // Method to set the graphics quality
        private void SetGraphicsQuality(int number)
        {
            // Set the graphics quality index in the GraphicsManager
            graphicsManager.Index.Index = number;

            // Set the quality level using Unity's QualitySettings
            QualitySettings.SetQualityLevel(number);

            // Update the displayed quality level text
            SetQualityText();
        }

        private void SetQualityText() 
        {
            qualityText.text = qualityTexts[graphicsManager.Index.Index].text;
        }

        public void ChangeTextCondition()
        {
            canTextChange = true;
        }
    }
}