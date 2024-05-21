using System.Collections;
using System.Collections.Generic;
using Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Managers
{
    public class KeyBoardUiManager : MonoBehaviour
    {
        [SerializeField] private KeyboardManager keyboardManager;
        [SerializeField] private List<GameObject> loadingSprite = new();
        [SerializeField] private KeyBoardConverterToImage keyBoardConverterToImage;
        [SerializeField] private List<Button> buttons = new();
        [SerializeField] private List<Image> keyImage;
        [SerializeField] private Sprite noneSprite;
        [SerializeField] private EventSystem system;


        private void Awake()
        {
            foreach (var sprite in loadingSprite) sprite.SetActive(false);
        }

        private void OnEnable()
        {
            keyboardManager.OnKeyOnRebindStart += ActivateLoadingImage;
            keyboardManager.OnKeOnRebindComplete += DeactivateLoadingImage;
            keyboardManager.OnInputChange += SetSprite;
            keyboardManager.OnSameKey += SetNoneIcon;
            keyboardManager.OnKeyReset += ResetIcons;
        }

        private void OnDisable()
        {
            keyboardManager.OnKeyOnRebindStart -= ActivateLoadingImage;
            keyboardManager.OnKeOnRebindComplete -= DeactivateLoadingImage;
            keyboardManager.OnInputChange -= SetSprite;
            keyboardManager.OnSameKey -= SetNoneIcon;
            keyboardManager.OnKeyReset -= ResetIcons;
        }

        private void Start()
        {
            for (var i = 0; i < keyImage.Count; i++)
            {
                keyImage[i].sprite =
                    ChooseSprite(keyboardManager.playerInput.actions.actionMaps[0].
                        actions[i].bindings[0].effectivePath);
            }
        }

        private void ActivateLoadingImage(int index)
        {
            loadingSprite[index].SetActive(true);
            buttons[index].interactable = false;
        }

        private void DeactivateLoadingImage(int index)
        {
            loadingSprite[index].SetActive(false);
            StartCoroutine(ResetInteractable(index));
        }

        private IEnumerator ResetInteractable(int index)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            buttons[index].interactable = true;
            system.SetSelectedGameObject(buttons[index].gameObject);
        }
        
        private void SetSprite(int index, string effectivePath)
        {
            keyImage[index].sprite = ChooseSprite(effectivePath);
        }

        private Sprite ChooseSprite(string effectivePath)
        {
            return keyBoardConverterToImage.GetSpriteForPath(effectivePath);
        }

        private void SetNoneIcon(int index)
        {
            keyImage[index].sprite = noneSprite;
        }

        private void ResetIcons()
        {
            for (var i = 0; i < keyImage.Count; i++)
            {
                keyImage[i].sprite =
                    ChooseSprite(
                        keyboardManager.playerInput.actions.actionMaps[0].actions[i].bindings[0].effectivePath);
            }
        }
    }
}