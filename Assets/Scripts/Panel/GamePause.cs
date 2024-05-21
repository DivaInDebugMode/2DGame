using System;
using Hover;
using Save_Load;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Panel
{
    // Manages game pause functionality
    public class GamePause : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel; // Reference to the pause panel
        [SerializeField] private int switchIndex; // Index used for managing panel state
        [SerializeField] private EventSystem eventSystem; // Reference to the event system
        [SerializeField] private GameObject audioListener; // Reference to the audio listener object
        [SerializeField] private Button resumeButton; // Reference to the resume button
        [SerializeField] private Button optionButton; // Reference to the options button
        [SerializeField] private Button backToMainMenuButton; // Reference to the button for returning to the main menu
        [SerializeField] private PlayerInput playerInput; // Reference to the player input system
        [SerializeField] private PauseCondition pauseCondition; // Reference to pause condition for UI state tracking

        // Event triggered when the game resumes
        public event Action OnResume;

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Add listeners to buttons
            resumeButton.onClick.AddListener(Resume);
            optionButton.onClick.AddListener(LoadOptionsScene);
            backToMainMenuButton.onClick.AddListener(SaveOnBackButton);
            
            // Deactivate pause panel and initialize switchIndex
            pausePanel.SetActive(false);
            switchIndex = 0;
        }

        // Called when the object becomes enabled and active
        private void OnEnable()
        {
            // Subscribe to the pause action and scene unloaded event
            if (playerInput != null) 
                playerInput.actions["Pause"].performed += SwitchPanel;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        // Called when the object becomes disabled or inactive
        private void OnDisable()
        {
            // Unsubscribe from the pause action and scene unloaded event
            if (playerInput != null) 
                playerInput.actions["Pause"].performed -= SwitchPanel;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        // Handles switching between pause and resume states
        private void SwitchPanel(InputAction.CallbackContext context)
        {
            // Check if the active scene is "DemoLevel" and options are not pressed
            if (SceneManager.GetActiveScene().name == "DemoLevel" && !pauseCondition.isOptionPressed)
            {
                switch (switchIndex)
                {
                    case 0:
                        // Pause the game
                        Time.timeScale = 0;
                        if (pausePanel != null) 
                            pausePanel.SetActive(true);
                        if (eventSystem != null) 
                            eventSystem.SetSelectedGameObject(resumeButton.gameObject);
                        switchIndex++;
                        break;
                    case 1:
                        // Resume the game
                        if (audioListener != null) 
                            audioListener.gameObject.SetActive(true);
                        if (pauseCondition != null) 
                            pauseCondition.isOptionPressed = false;
                        OnResume?.Invoke();
                        if (eventSystem != null) 
                            eventSystem.gameObject.SetActive(true);
                        Time.timeScale = 1;
                        if (pausePanel != null) 
                            pausePanel.SetActive(false);
                        switchIndex--;
                        break;
                }
            }
        }

        // Resumes the game
        private void Resume()
        {
            OnResume?.Invoke();
            pauseCondition.isOptionPressed = false;
            eventSystem.gameObject.SetActive(true);
            audioListener.gameObject.SetActive(true);
            Time.timeScale = 1;
            pausePanel.gameObject.SetActive(false);
            switchIndex--;
        }

        // Loads the options scene
        private void LoadOptionsScene()
        {
            pauseCondition.isOptionPressed = true;
            eventSystem.gameObject.SetActive(false);
            audioListener.gameObject.SetActive(false);
            playerInput.enabled = false;
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Additive);
        }
        
        // Called when a scene is unloaded
        private void OnSceneUnloaded(Scene unloadedScene)
        {
            // Reset option state and activate event system and audio listener
            pauseCondition.isOptionPressed = false;
            eventSystem.gameObject.SetActive(true);
            audioListener.SetActive(true);
            playerInput.enabled = true;
        }

        // Called when the application is quitting
        private void OnApplicationQuit()
        {
            // Save the game on back button press
            SaveOnBackButton();
        }

        // Saves the game
        private void SaveOnBackButton()
        {
            DataPersistenceManager.Instance.SaveGame();
        }
    }
}
