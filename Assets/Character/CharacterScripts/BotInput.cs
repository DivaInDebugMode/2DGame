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
        
        public InputActionReference Run => run;
        public InputActionReference MoveLeft => moveLeft;
        public InputActionReference MoveRight => moveRight;
        public InputActionReference MoveDown => moveDown;
    }
}
