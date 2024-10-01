using Panel;
using UnityEngine;
using UnityEngine.InputSystem;

// Namespace for control-related functionality
namespace Control
{
    // Class responsible for applying input override paths to player input actions
    public class ApplyOverridePath : MonoBehaviour
    {
        // Reference to the player input component
        [SerializeField] private PlayerInput characterInput;
        
        // Reference to the input override path containing controller inputs
        [SerializeField] private InputOverridePath inputOverridePath;
        
        // Reference to the game pause component
        [SerializeField] private GamePause gamePause;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Apply inputs immediately
            ApplyInputs();
        }

        // Called when the object becomes enabled and active
        private void OnEnable()
        {
            // Subscribe to the game pause resume event to reapply inputs
            gamePause.OnResume += ApplyInputs;
        }

        // Called when the object is being deactivated
        private void OnDisable()
        {
            // Unsubscribe from the game pause resume event
            gamePause.OnResume -= ApplyInputs;
        }

        // Method to apply keyboard inputs when the game resumes
        private void ApplyInputs()
        {
            // Loop through keyboard inputs and apply binding overrides
            for (var i = 0; i < inputOverridePath.KeyboardInputs.Count; i++)
            {
                characterInput.actions.actionMaps[0].actions[i].ApplyBindingOverride(
                    0, // Keyboard index
                    new InputBinding { overridePath = inputOverridePath.KeyboardInputs[i] }
                );
            }
            
            for (var i = 0; i < inputOverridePath.ControllerInputs.Count; i++)
            {
                characterInput.actions.actionMaps[0].actions[i].ApplyBindingOverride(
                    1, // Controller index
                    new InputBinding { overridePath = inputOverridePath.ControllerInputs[i] }
                );
            }
        }
    }
}