using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private CapsuleCollider moveCollider;
        public Rigidbody Rb => rigidbody;
        public CapsuleCollider MoveCollider => moveCollider;
    }
}
