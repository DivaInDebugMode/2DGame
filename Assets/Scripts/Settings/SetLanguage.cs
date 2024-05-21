using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    // Allows users to set the language using buttons
    public class SetLanguage : MonoBehaviour
    {
        [SerializeField] private List<Button> languageButtons = new List<Button>(); // List of language selection buttons
        [SerializeField] private LanguageManager languageManager; // Reference to the language manager
        public event Action OnLanguageChange;
        
        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Iterate through each language button
            for (var i = 0; i < languageButtons.Count; i++)
            {
                var index = i;
                // Add a listener to each button to change language
                languageButtons[i].onClick.AddListener(() => ChangeLanguage(index));
            }
        }

        // Changes the language based on the selected index
        private void ChangeLanguage(int index)
        {
            // Set the language index in the language manager
            languageManager.LanguageIndex.Index = index;
            OnLanguageChange?.Invoke();
            // Check if the index is within valid bounds
            if (index < 0 || index >= languageManager.Locales.Count)
                return;
            
            // Set the selected locale in the language manager settings
            if (languageManager.Settings.GetSelectedLocale() != languageManager.Locales[index])
                languageManager.Settings.SetSelectedLocale(languageManager.Locales[index]);
        }
    }
}