using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveLeft;
        [SerializeField] private InputActionReference moveRight;
        [SerializeField] private InputActionReference moveDown;
        [SerializeField] private InputActionReference run;
        [SerializeField] private InputActionReference dash;
        
        public InputActionReference Run => run;
        public InputActionReference Dash => dash;
        public InputActionReference MoveLeft => moveLeft;
        public InputActionReference MoveRight => moveRight;
        public InputActionReference MoveDown => moveDown;
    }
}
