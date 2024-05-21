using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerMovement
{
    /// <summary>
    /// Handles input actions for the character.
    /// </summary>
    public class CharacterInput : MonoBehaviour
    {
        [Header("Character Actions")] 
        [SerializeField] private InputActionReference moveRight;
        [SerializeField] private InputActionReference moveLeft;
        [SerializeField] private InputActionReference moveUp;
        [SerializeField] private InputActionReference moveDown;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference dashAction;
        [SerializeField] private InputActionReference attackAction;

        // Properties to access input actions
        public InputActionReference JumpAction => jumpAction;
        public InputActionReference DashAction => dashAction;
        public InputActionReference MoveUp
        {
            get => moveUp;
            set => moveUp = value;
        }
        public InputActionReference MoveRight => moveRight;
        public InputActionReference MoveLeft => moveLeft;
        public InputActionReference MoveDown => moveDown;
        public InputActionReference Attack => attackAction;

        // Event for jump action
        public event Action<InputAction.CallbackContext> OnJumpAction;

        private void OnEnable()
        {
            // Subscribe to jump action performed event
            jumpAction.action.performed += JumpActionPressed;
        }

        private void OnDisable()
        {
            // Unsubscribe from jump action performed event
            jumpAction.action.performed -= JumpActionPressed;
        }

        // Method to handle jump action
        private void JumpActionPressed(InputAction.CallbackContext context)
        {
            // Invoke jump action event
            OnJumpAction?.Invoke(context);
        }
    }
}
