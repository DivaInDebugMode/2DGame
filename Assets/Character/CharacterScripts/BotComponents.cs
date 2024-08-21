using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotComponents : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private BoxCollider moveCollider;
       
        public Rigidbody Rb => rigidbody;
        public BoxCollider MoveCollider => moveCollider;

       
    }
}