using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    // Manages switching between scenes
    public class SwitchSceneManager : MonoBehaviour
    {
        [SerializeField] private SceneIndex sceneIndex; // Reference to the current scene index

        // Called when the object becomes enabled and active
        private void Awake()
        {
            // Set the initial scene index to the index of the currently active scene
            sceneIndex.Index = SceneManager.GetActiveScene().buildIndex;
        }

        // Switches scenes based on the provided index
        public void SwitchScenes(int index)
        {
            switch (index)
            {
                // Case for switching to the start menu scene
                case 0:
                    // Set the scene index to the specified value
                    sceneIndex.Index = index;
                    // Load the start menu scene
                    SceneManager.LoadScene("Scenes/StartMenu");
                    break;

                // Case for switching to the demo level scene
                case 1:
                    // Reset time scale to normal
                    Time.timeScale = 1;
                    // Set the scene index to the specified value
                    sceneIndex.Index = index;
                    // Load the demo level scene
                    SceneManager.LoadScene("Scenes/DemoLevel");
                    break;
            }
        }
    }
}
