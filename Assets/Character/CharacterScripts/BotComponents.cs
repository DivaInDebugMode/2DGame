using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private CapsuleCollider moveCollider;
        [SerializeField] private CapsuleCollider crouchCollider;

        public Rigidbody Rb => rigidbody;

        public CapsuleCollider CrouchCollider => crouchCollider;

        public CapsuleCollider MoveCollider => moveCollider;
    }
}
