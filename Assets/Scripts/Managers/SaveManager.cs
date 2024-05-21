using Graphics;
using Settings;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    // Manages saving and loading game settings
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private LanguageIndex languageIndex; // The selected language index
        [SerializeField] private GraphicsIndex graphicsIndex; // The selected graphics index
        [SerializeField] private SoundValue soundValue; // The volume level
        [SerializeField] private BrightnessValue brightnessValue; // The brightness level
        [SerializeField] private Button saveButton; // The back button reference

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Load settings when the game starts
            LoadSettings();
            // Listen to the back button click event to save settings
            saveButton.onClick.AddListener(SaveSettings);
        }

        // Saves the current game settings to PlayerPrefs
        private void SaveSettings()
        {
            // Save brightness value
            PlayerPrefs.SetFloat("Brightness", brightnessValue.Value);
            // Save selected language index
            PlayerPrefs.SetInt("Language", languageIndex.Index);
            // Save selected graphics index
            PlayerPrefs.SetInt("Graphics", graphicsIndex.Index);
            // Save sound volume
            PlayerPrefs.SetFloat("Volume", soundValue.Amount);
            // Save PlayerPrefs changes
            PlayerPrefs.Save();
        }

        // Loads game settings from PlayerPrefs
        private void LoadSettings()
        {
            // Load saved language index from PlayerPrefs, default to 0 if not found
            var savedLanguage = PlayerPrefs.GetInt("Language", 0);
            languageIndex.Index = savedLanguage;
            // Load saved brightness value from PlayerPrefs, default to 1.0 if not found
            var savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
            brightnessValue.Value = savedBrightness;
            // Load saved graphics index from PlayerPrefs, default to 2 if not found
            var savedGraphics = PlayerPrefs.GetInt("Graphics", 2);
            graphicsIndex.Index = savedGraphics;
            // Load saved sound volume from PlayerPrefs, default to 0 if not found
            var savedSound = PlayerPrefs.GetFloat("Volume", 0);
            soundValue.Amount = savedSound;
        }
    }
}