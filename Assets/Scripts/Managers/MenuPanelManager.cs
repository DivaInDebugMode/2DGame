using System.Collections.Generic;
using System.Linq;
using Hover;
using Save_Load;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Namespace declaration
namespace Managers
{
    // Class declaration
    public class MenuPanelManager : MonoBehaviour
    {
        // Serialized fields for Unity inspector
        [SerializeField] private EventSystem eventSystem; // Event system for UI navigation
        [SerializeField] private Button answerButtonYes;

        [SerializeField] private List<Button> firstButtonsOfMainPanels = new();
        [SerializeField] private List<GameObject> panels = new();

        [SerializeField] private GameObject backQuestionPanel;
        [SerializeField] private Button answerButtonNo;
        [SerializeField] private List<TextMeshProUGUI> questionText = new();
        [SerializeField] private TextMeshProUGUI currentQuestion;

        [SerializeField] private PlayerInput playerInput; // Player input reference
        [SerializeField] private List<OptionButtonHover> optionButtonHovers = new(); // List of option button hovers
        [SerializeField] private List<GameObject> innerOptionPanels; // List of main panels
        [SerializeField] private SceneIndex sceneIndex; // Index of the current scene
        [SerializeField] private GameIndex gameIndex; // Index of the current game
        [SerializeField] private PauseCondition pauseCondition; // Pause condition reference
        [SerializeField] private Button backButton; // Button to go back
        private GameObject lastSelected;
        [SerializeField] private PanelTransitionSound panelTransitionSound;
        private void Awake()
        {
            answerButtonYes.onClick.AddListener(BackToAction);
            answerButtonNo.onClick.AddListener(DisableQuestionPanel);
            backButton.onClick.AddListener(EnableQuestionPanel);
            switch (sceneIndex.Index)
            {
                case 0: 
                    panels[0].SetActive(true);
                    panels[1].SetActive(false);
                    panels[2].SetActive(false);
                    eventSystem.SetSelectedGameObject(firstButtonsOfMainPanels[0].gameObject);
                    break;
                case > 0: 
                    panels[0].SetActive(false);
                    panels[1].SetActive(true);
                    panels[2].SetActive(false);
                    break;
            }
        }

        private void OnEnable()
        {
            if (playerInput != null) playerInput.actions["Back"].performed += BackWithAction;
        }
        
        private void OnDisable()
        {
            if (playerInput != null) playerInput.actions["Back"].performed -= BackWithAction;
        }

        private void BackWithAction(InputAction.CallbackContext context)
        {
            if (!backQuestionPanel.activeSelf && panels[1].activeSelf)
            {
                for (var i = 0; i < optionButtonHovers.Count; i++)
                {
                    if ((optionButtonHovers[i].ActiveIndex == -1 && innerOptionPanels[i].activeSelf)
                        || eventSystem.currentSelectedGameObject == backButton.gameObject)
                    {
                        var obj = eventSystem.currentSelectedGameObject;
                        lastSelected = obj;
                        EnableQuestionPanel();
                    }
                }
            }
            else if(panels[1].activeSelf)
            {
                DisableQuestionPanel();
            }
        }

        private void BackToAction()
        {
            backQuestionPanel.SetActive(false);
            SwitchPanels(0);
        }
        
        public void SwitchPanels(int index)
        {
            foreach (var t in panels.Where(t => t.activeSelf))
            {
                t.SetActive(false);
                panelTransitionSound.PanelSound();
            }

            switch (sceneIndex.Index)
            {
                case 0:
                    panels[index].SetActive(true);
                    eventSystem.SetSelectedGameObject(firstButtonsOfMainPanels[index].gameObject);
                    break;
                case > 0:
                    pauseCondition.isOptionPressed = false; 
                    ReturnToGame();
                    break;
            }
        }

        private void DisableQuestionPanel()
        {
            eventSystem.SetSelectedGameObject(lastSelected.gameObject);
            backQuestionPanel.SetActive(false);
        }

        private void EnableQuestionPanel()
        {
            currentQuestion.text = sceneIndex.Index switch
            {
                0 => questionText[sceneIndex.Index].text,
                > 0 => questionText[sceneIndex.Index].text,
                _ => currentQuestion.text
            };
            backQuestionPanel.SetActive(true);
            eventSystem.SetSelectedGameObject(answerButtonYes.gameObject);
        }


        public void StartNewGame(int index) => gameIndex.Index = index;

        public void LoadGameWithButtonIndex(int index) => gameIndex.Index = index;

        private void ReturnToGame() => SceneManager.UnloadSceneAsync("StartMenu");
    }
}
