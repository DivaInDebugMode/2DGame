using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

// Namespace declaration
namespace Managers
{
    // Class declaration
    public class LanguageManager : MonoBehaviour
    {
        // Serialized field for Unity inspector
        [SerializeField] private LocalizationSettings settings; // Localization settings reference
        public LocalizationSettings Settings => settings; // Public property to access settings

        // Serialized field for Unity inspector
        [SerializeField] private List<Locale> locales = new List<Locale>(); // List of available locales
        public List<Locale> Locales => locales; // Public property to access locales

        // Serialized field for Unity inspector
        [SerializeField] private LanguageIndex languageIndex; // Index of the current language
        public LanguageIndex LanguageIndex => languageIndex; // Public property to access language index

        // Awake method called before Start
        private void Awake()
        {
            ChangeLanguage(); // Call method to change language on awake
        }

        // Method to change the language
        public void ChangeLanguage()
        {
            // Set the selected locale in localization settings based on language index
            settings.SetSelectedLocale(locales[languageIndex.Index]);
        }
    }
}
