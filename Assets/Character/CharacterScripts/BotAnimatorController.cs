using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator => animator;
    }
}