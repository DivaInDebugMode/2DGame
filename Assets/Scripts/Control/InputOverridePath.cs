using System.Collections.Generic;
using UnityEngine;

// Namespace for control-related functionality
namespace Control
{
    // Class responsible for managing input override paths
    public class InputOverridePath : MonoBehaviour
    {
        // List of controller input override paths
        [SerializeField] private List<string> controllerInputs = new List<string>();

        // List of keyboard input override paths
        [SerializeField] private List<string> keyboardInputs = new List<string>();

        // Property to expose controller input override paths
        public List<string> ControllerInputs => controllerInputs;

        // Property to expose keyboard input override paths
        public List<string> KeyboardInputs => keyboardInputs;
    }
}