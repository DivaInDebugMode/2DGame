using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Namespace declaration
namespace Managers
{
    // Class declaration
    public class ResolutionManager : MonoBehaviour
    {
        // Serialized fields for Unity inspector
        [SerializeField] private List<ResolutionObject> resolutions = new List<ResolutionObject>(); // List of available resolutions
        [SerializeField] private List<Button> resolutionButtons = new List<Button>(); // List of resolution buttons
        [SerializeField] private Toggle toggle; // Toggle for fullscreen mode
        [SerializeField] private TextMeshProUGUI currentResolutionText; // Text for displaying current resolution

        // Awake method called before Start
        private void Awake()
        {
            // Iterate through resolutions
            for (var i = 0; i < resolutions.Count; i++)
            {
                var index = i;
                // Add listener to each resolution button to set resolution
                resolutionButtons[i].onClick.AddListener(() => SetIndexResolution(index));
            }
        
            // Display current screen resolution
            currentResolutionText.text = Screen.width + "x" + Screen.height;
        }
    
        // Method to set resolution based on index
        private void SetIndexResolution(int index)
        {
            // Set resolution with specified horizontal and vertical dimensions and toggle for fullscreen
            Screen.SetResolution(resolutions[index].horizontal, resolutions[index].vertical, toggle.isOn);
            // Update current resolution text
            currentResolutionText.text = resolutions[index].horizontal + "x" + resolutions[index].vertical;
        }
    }

    // Class for storing resolution data
    [System.Serializable]
    public class ResolutionObject
    {
        // Public fields for horizontal and vertical resolution
        public int horizontal, vertical;
    }
}
