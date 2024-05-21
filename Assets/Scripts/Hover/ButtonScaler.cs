using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hover
{
    public class ButtonScaler : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        private GameObject lastSelectedObj;
        private readonly Vector3 defaultSize = new(2.0f, 2.0f, 1f);
        private readonly Vector3 hoverSize = new(2.1f, 2.1f, 1f);
        private bool hasChanged;

        private void Start()
        {
            lastSelectedObj = eventSystem.currentSelectedGameObject;
            lastSelectedObj.transform.localScale = hoverSize;
        }

        private void Update()
        {
            if (eventSystem.currentSelectedGameObject == lastSelectedObj) return;
            lastSelectedObj.transform.localScale = defaultSize;
            lastSelectedObj = eventSystem.currentSelectedGameObject;
            lastSelectedObj.transform.localScale = hoverSize;
        }
    }
}