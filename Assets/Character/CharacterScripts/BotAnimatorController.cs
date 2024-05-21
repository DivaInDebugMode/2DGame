using UnityEngine;
using UnityEngine.Serialization;

namespace Character.CharacterScripts
{
    public class BotAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public string botIdleAnim = "StandartIdle";
        public string botRunAnim = "Running";
        public string botRunToStopAnim = "Run To Stop"; 
        public string botRunningTurnLeft = "Running Turn Left";
        public string botRunningTurnRight = "Running Turn Right";
        public string fallingEdge = "EdgeStop";
        private string currentAnimation; // Currently playing animation
        

        // Changes the character's animation state with a transition
        public void ChangeAnimationState(string newStateAnim, float transitionTime)
        {
            // Skip if the new animation state is already playing
            if (currentAnimation == newStateAnim) return;

            // Crossfade to the new animation state with the specified transition time
            animator.CrossFade(newStateAnim, transitionTime);

            // Update the current animation state
            currentAnimation = newStateAnim;
        }

        private void TurnOnRoot() => animator.applyRootMotion = true;
        private void TurnOfRoot() => animator.applyRootMotion = false;
    }
}