using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference moveLeft;
        [SerializeField] private InputActionReference moveRight;

        public InputActionReference MoveLeft => moveLeft;
        public InputActionReference MoveRight => moveRight;
    }
}
