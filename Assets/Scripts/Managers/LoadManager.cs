using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
{
    [Serializable]
    public class SaveData
    {
        // Serializable class to hold data to be saved
        public List<string> buttonTexts; // List to store text of buttons
    }

    public class LoadManager : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons = new List<Button>(); // List of buttons to manage
        [SerializeField] private List<TextMeshProUGUI> buttonText = new List<TextMeshProUGUI>(); // List of text components for button texts

        // Get the file path to save data
        private static string FilePath => Path.Combine(Application.dataPath, "SaveFiles/activeGameProfiles.json");

        // Called when the script instance is being loaded
        private void Awake()
        {
            // Loop through all buttons
            for (var i = 0; i < buttons.Count; i++)
            {
                var index = i;
                // Get or add EventTrigger component to the button's GameObject
                var eventTrigger = buttons[i].gameObject.GetComponent<EventTrigger>()
                                   ?? buttons[i].gameObject.AddComponent<EventTrigger>();

                // Create a new EventTrigger entry for submit event
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Submit
                };
                // Add a listener to the submit event that calls ActivateIndex method with the button's index
                entry.callback.AddListener((_) => ActivateIndex(index));
                eventTrigger.triggers.Add(entry);
            }

            // Load data when the script is awakened
            LoadData();
        }

        // Method to activate index and update button text
        private void ActivateIndex(int index)
        {
            // If button text matches the default text, return
            if (buttonText[index].text == $"Load User {index + 1}") return;
            // Update button text
            buttonText[index].text = $"Load User {index + 1}";
            // Save data
            SaveData();
        }

        // Method to save data to file
        private void SaveData()
        {
            // Create a new instance of SaveData and populate it with button texts
            var saveData = new SaveData
            {
                buttonTexts = buttonText.Select(text => text.text).ToList()
            };

            try
            {
                // Serialize saveData to JSON and write it to the file
                var save = JsonUtility.ToJson(saveData);
                File.WriteAllText(FilePath, save);
            }
            catch (Exception e)
            {
                // Handle exceptions
                Debug.LogError($"Failed to save data: {e}");
            }
        }

        // Method to load data from file
        private void LoadData()
        {
            // If the file does not exist, return
            if (!File.Exists(FilePath)) return;
            try
            {
                // Read JSON data from the file and deserialize it into a SaveData object
                var load = File.ReadAllText(FilePath);
                var saveData = JsonUtility.FromJson<SaveData>(load);

                // Update button texts with the loaded data
                for (int i = 0; i < buttonText.Count && i < saveData.buttonTexts.Count; i++)
                    buttonText[i].text = saveData.buttonTexts[i];
            }
            catch (Exception e)
            {
                // Handle exceptions
                Debug.LogError($"Failed to load data: {e}");
            }
        }

        // Method to clear data for a specific index
        public void ClearData(int index)
        {
            // Get the file path for the given index
            var path = GetSaveFilePath(index);

            // If the file exists, delete it
            if (File.Exists(path)) File.Delete(path);

            // Save new data for the index and update button text
            SaveNewData(index);
            buttonText[index].text = $"Empty User {index + 1}";

            // Save data after clearing
            SaveData();
        }

        // Method to save new data for a specific index
        private void SaveNewData(int index)
        {
            // Get file paths
            var filePath = GetSaveFilePath(index);
            var fullPath = Path.Combine(Application.dataPath, "SaveFiles/Example.json");
            
            try
            {
                // If the file exists, delete it
                if (File.Exists(filePath)) File.Delete(filePath);
                // Copy example file to the specified location
                File.Copy(fullPath, filePath);
            }
            catch (Exception e)
            {
                // Handle exceptions
                Debug.LogError($"Failed to save new data: {e}");
            }
        }

        // Method to get the file path for a specific index
        private string GetSaveFilePath(int index)
        {
            return Path.Combine(Application.persistentDataPath, $"LoadGame{index + 1}.json");
        }
    }
}