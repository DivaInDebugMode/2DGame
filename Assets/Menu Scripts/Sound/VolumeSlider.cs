using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(_ => VolumeSliderSound());
        }
        
        private void VolumeSliderSound()
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}