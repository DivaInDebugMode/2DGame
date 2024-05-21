using System.Collections.Generic;
using Camera;
using Player.PlayerMovement;
using UnityEngine;

namespace Save_Load
{
    // Manages data persistence for saving and loading game data
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage cfg")] 
        [SerializeField] private List<string> gameFileName = new List<string>(); // List of game file names
        
        // Reference to the current game data
        public GameData gameData;

        // References to other components for data management
        [SerializeField] private CharacterStateMachine characterStateMachine;
        [SerializeField] private CameraFollow cameraFollow;

        // Instance of the data persistence manager
        private FileDataHandler fileDataHandler;

        // Reference to the current game index
        [SerializeField] private GameIndex gameIndex;

        // Static instance of the data persistence manager
        public static DataPersistenceManager Instance { get; private set; }

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Ensure only one instance of the data persistence manager exists
            if (Instance == null) 
                Instance = this;
            
            // Initialize the file data handler
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, gameFileName, gameIndex.Index);
        }

        // Called when the object is initialized
        private void Start()
        {
            // Load the game data on start
            LoadGame();
        }   

        // Starts a new game
        public void NewGame()
        {
            // Load game data from file or create new if not found
            gameData = fileDataHandler.Load(gameIndex.Index);
            gameData = new GameData();
        }

        // Loads the game data
        public void LoadGame()
        {
            // Load game data from file or create new if not found
            gameData = fileDataHandler.Load(gameIndex.Index);
            if (gameData == null) 
                NewGame();
            
            // Load data for character state machine and camera follow components
            characterStateMachine.LoadData(gameData);
            cameraFollow.LoadData(gameData);
        }

        // Saves the game data
        public void SaveGame()
        {
            // Save data from character state machine and camera follow components
            characterStateMachine.SaveData(ref gameData);
            cameraFollow.SaveData(ref gameData);
            
            // Save game data to file
            fileDataHandler.SaveGame(gameData);
        }
    }
}