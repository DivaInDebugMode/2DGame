using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hover
{
    public class OptionButtonUnHover : MonoBehaviour
    {
        // Serialized fields can be set in the Unity Inspector
        [SerializeField] private List<Button> buttons = new(); // List of buttons to track
        [SerializeField] private EventSystem eventSystem; // Reference to the Event System
        [SerializeField] private GameObject mainButton;
        private bool isSelected;

        private void Update()
        {
            if (eventSystem.currentSelectedGameObject != mainButton || isSelected) return;
            isSelected = true;
            if (!isSelected) return;
            foreach (var childButtons in buttons)
            {
                childButtons.targetGraphic.color = Color.grey;
            }
        }

        public void ReturnHoverToParentButton()
        {
            isSelected = false;
        }
    }
}