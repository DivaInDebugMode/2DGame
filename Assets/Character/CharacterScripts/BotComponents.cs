using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private new BoxCollider collider;

        public Rigidbody Rb
        {
            get => rigidbody;
            set => rigidbody = value;
        }

        public Animator Animator
        {
            get => animator;
            set => animator = value;
        }

        public BoxCollider Coll
        {
            get => collider;
            set => collider = value;
        }
    }
}
