using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private new CapsuleCollider collider;
        [SerializeField] private BoxCollider boxCollider;

        public Rigidbody Rb
        {
            get => rigidbody;
            set => rigidbody = value;
        }

        public BoxCollider BoxCollider
        {
            get => boxCollider;
            set => boxCollider = value;
        }

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public CapsuleCollider Coll
        {
            get => collider;
            set => collider = value;
        }
    }
}
