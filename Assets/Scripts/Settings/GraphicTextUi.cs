using Graphics;
using UnityEngine;

namespace Settings
{
    public class GraphicTextUi : MonoBehaviour
    {
        [SerializeField] private SetLanguage setLanguage;
        [SerializeField] private SetGraphics setGraphics;

        private void OnEnable()
        {
            setLanguage.OnLanguageChange += setGraphics.ChangeTextCondition;
        }

        private void OnDisable()
        {
            setLanguage.OnLanguageChange -= setGraphics.ChangeTextCondition;
        }
    }
}