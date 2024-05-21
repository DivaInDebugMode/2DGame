using UnityEngine;

namespace Player.PlayerMovement
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private CharacterStats characterStats;

        private void CancelSwordAttack()
        {
            characterStats.IsAttacking = false;
        }
    }
}