using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new CapsuleCollider collider;
        [SerializeField] private BoxCollider crouchCollider;
        [SerializeField] private BoxCollider airDashCollider;
        [SerializeField] private BoxCollider jumpFallCollider;

        public BoxCollider AirDashCollider
        {
            get => airDashCollider;
        }

        public BoxCollider JumpFallCollider
        {
            get => jumpFallCollider;
        }

        public Rigidbody Rb
        {
            get => rigidbody;
        }

        public BoxCollider CrouchCollider
        {
            get => crouchCollider;
        }

        public CapsuleCollider Coll
        {
            get => collider;
        }
    }
}
