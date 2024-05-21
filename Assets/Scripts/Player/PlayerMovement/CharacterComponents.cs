using UnityEngine;

namespace Player.PlayerMovement // Namespace for managing player movement
{
    // Class representing various components of the character, inheriting from MonoBehaviour
    public class CharacterComponents : MonoBehaviour
    {
        [Header(("Rigidbody2D/Sr/Col"))] // Header for organization in Unity Inspector
        [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component
        [SerializeField] private SpriteRenderer sr; // Reference to the SpriteRenderer component
        [SerializeField] private SpriteRenderer ledgeFinishRight; // Reference to the right ledge finish SpriteRenderer
        [SerializeField] private SpriteRenderer ledgeFinishLeft; // Reference to the left ledge finish SpriteRenderer
        [SerializeField] private CapsuleCollider2D col; // Reference to the CapsuleCollider2D component
        [SerializeField] private BoxCollider2D attackCollider; // Reference to the attack BoxCollider2D component
        [SerializeField] private BoxCollider2D upAttackCollider; // Reference to the upward attack BoxCollider2D component
        private bool flipped; // Flag for checking if character is flipped

        // Property to access Rigidbody2D component
        public Rigidbody2D Rb => rb;

        // Property to access SpriteRenderer component
        public SpriteRenderer Sr => sr;

        // Property to access right ledge finish SpriteRenderer component
        public SpriteRenderer LedgeFinishRight => ledgeFinishRight;

        // Property to access left ledge finish SpriteRenderer component
        public SpriteRenderer LedgeFinishLeft => ledgeFinishLeft;

        // Property to access CapsuleCollider2D component with getter and setter
        public CapsuleCollider2D Col
        {
            get => col;
            set => col = value;
        }

        // Method called on every frame update
        private void Update()
        {
            // Sets position of attack colliders based on flip status
            SettAttackColTransform();
        }

        // Method to set attack collider transforms based on flip status
        private void SettAttackColTransform()
        {
            switch (sr.flipX)
            {
                case true when !flipped:
                {
                    var transform1 = attackCollider.transform;
                    var position = transform1.position;
                    position = new Vector3(position.x - 1.4f, position.y);
                    transform1.position = position;
                    var transform2 = upAttackCollider.transform;
                    var position1 = transform2.position;
                    position1 = new Vector3(position1.x - 0.7f, position1.y);
                    transform2.position = position1;
                    flipped = true;
                    break;
                }
                case false when flipped:
                {
                    var transform1 = attackCollider.transform;
                    var position = transform1.position;
                    position = new Vector3(position.x + 1.4f, position.y);
                    transform1.position = position;
                    var transform2 = upAttackCollider.transform;
                    var position1 = transform2.position;
                    position1 = new Vector3(position1.x + 0.7f, position1.y);
                    transform2.position = position1;
                    flipped = false;
                    break;
                }
            }
        }
    }
}
