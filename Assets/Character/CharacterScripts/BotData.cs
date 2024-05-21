using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotData : MonoBehaviour
    {
        [SerializeField] private BotStats botStats;
        [SerializeField] private BotDetectionStats botDetectionStats;
        [SerializeField] private BotComponents botComponents;
        [SerializeField] private BotDetection botDetection;

        public BotStats BotStats => botStats;
        public BotDetectionStats BotDetectionStats => botDetectionStats;
        public BotComponents BotComponents => botComponents;

        public BotDetection BotDetection => botDetection;
    }
}