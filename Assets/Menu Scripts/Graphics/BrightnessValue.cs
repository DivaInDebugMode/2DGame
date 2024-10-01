using UnityEngine;

namespace Graphics
{
    public class BrightnessValue : MonoBehaviour
    {
        [SerializeField] private float value;
        public float Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
