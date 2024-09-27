using Graphics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Managers
{
    public class BrightnessManager : MonoBehaviour
    {
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private PostProcessProfile postProcessProfile;
        [SerializeField] private BrightnessValue brightnessValue;
        private AutoExposure _exposure;

       
        private void Awake()
        {
            brightnessSlider.value = brightnessValue.Value;
            postProcessProfile.TryGetSettings(out _exposure);
            SetBrightness(brightnessValue.Value);
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        public void SetBrightness(float value)
        {
            _exposure.keyValue.value = value != 0 ? value : 0.05f;
            brightnessValue.Value = value;
        }
    }
}
