using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Save_Load
{
    // Handles loading and saving of game data from and to files
    public class FileDataHandler
    {
        // Path where the data files are stored
        private readonly string dataPath;

        // List of data file names
        private readonly List<string> dataFile;

        // Index to determine which data file to use
        private readonly int index;

        // Constructor to initialize the file data handler
        public FileDataHandler(string dataPath, List<string> dataFile, int index)
        {
            this.dataPath = dataPath;
            this.dataFile = dataFile;
            this.index = index;
        }
    
        // Loads game data from a file
        public GameData Load(int loadIndex)
        {
            // Combine data path and file name to get full file path
            var fullPath = Path.Combine(dataPath, dataFile[loadIndex]);

            // Initialize loaded data as null
            GameData loadedData = null;

            // Check if the file exists
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad;

                    // Read the entire file contents
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                            dataToLoad = reader.ReadToEnd();
                    }
                
                    // Deserialize the JSON data into a GameData object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    // Log error if loading fails
                    Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
                }
            }
    
            // Return the loaded data (null if loading failed)
            return loadedData;
        }
    
        // Saves game data to a file
        public void SaveGame(GameData data)
        {
            // Combine data path and file name to get full file path
            var fullPath = Path.Combine(dataPath, dataFile[index]);
            try
            {
                // Create directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
    
                // Serialize the game data into JSON format
                var dataToStore = JsonUtility.ToJson(data, true);

                // Write the serialized data to the file
                using var stream = new FileStream(fullPath, FileMode.Create);
                using var writer = new StreamWriter(stream);
                writer.Write(dataToStore);
            }
            catch (Exception)
            {
                // Log error if saving fails
                Debug.LogError("Error occurred when trying to save data to file: " + fullPath);
            }
        }
    }
}