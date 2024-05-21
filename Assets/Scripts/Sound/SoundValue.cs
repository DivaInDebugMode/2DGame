using UnityEngine;

namespace Sound
{
    public class SoundValue : MonoBehaviour
    {
        [SerializeField] private float amount;
        public float Amount
        {
            get => amount;
            set => amount = value;
        }
    }
}