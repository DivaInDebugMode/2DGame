using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new CapsuleCollider collider;
        [SerializeField] private BoxCollider crouchCollider;
        [SerializeField] private BoxCollider airDashCollider;

        public BoxCollider AirDashCollider
        {
            get => airDashCollider;
        }

        public Rigidbody Rb
        {
            get => rigidbody;
        }

        public BoxCollider BoxCollider
        {
            get => crouchCollider;
        }

        public CapsuleCollider Coll
        {
            get => collider;
        }
    }
}
