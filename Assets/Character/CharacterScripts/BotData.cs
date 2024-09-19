using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotData : MonoBehaviour
    {
        [SerializeField] private BotStats botStats;
        [SerializeField] private BotDetectionStats botDetectionStats;
        [SerializeField] private BotDetection botDetection;
        [SerializeField] private Animator animator;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private BoxCollider moveCollider;
        
        public BotStats BotStats => botStats;
        public BotDetectionStats BotDetectionStats => botDetectionStats;
        public BotDetection BotDetection => botDetection;
        public Animator Animator => animator;
        public Rigidbody Rb => rigidbody;
        public BoxCollider MoveCollider => moveCollider;
    }
}