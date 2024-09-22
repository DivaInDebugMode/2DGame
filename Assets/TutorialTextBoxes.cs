using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class TutorialTextBoxes : MonoBehaviour
{
   [SerializeField] private LayerMask player;
    [SerializeField] private GameObject textMeshProObj;
    [SerializeField] private InputActionReference inputActionReference;
    [SerializeField] private TextMeshPro textMeshProUGUI;
    [SerializeField] private List<string> inputText;
    [SerializeField] private bool hasPressedButton;
    
    private InputDevice lastInputDevice;

    private void Start()
    {
        inputActionReference.action.Disable();
        textMeshProObj.transform.localScale = Vector3.zero;
        hasPressedButton = false;
        UpdateBindingText();
    }

    private void Update()
    {
        var currentDevice = GetCurrentInputDevice();
        if (currentDevice == lastInputDevice) return;
        lastInputDevice = currentDevice;
        UpdateBindingText();
    }

    private InputDevice GetCurrentInputDevice()
    {
        if (Gamepad.current is { wasUpdatedThisFrame: true })
        {
            return Gamepad.current;
        }

        return Keyboard.current is { wasUpdatedThisFrame: true } ? Keyboard.current : lastInputDevice;
    }

    private void UpdateBindingText()
    {
        var bindingIndex = (lastInputDevice is Gamepad) ? 1 : 0;

        textMeshProUGUI.text = inputText[0] + inputActionReference.action.bindings[bindingIndex].ToDisplayString() + inputText[1] + inputText[2] + inputText[3] + inputText[4];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPressedButton) return;
        if (((1 << other.gameObject.layer) & player.value) != 0)
        {
            inputActionReference.action.Enable();
            textMeshProUGUI.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(hasPressedButton) return;
        if (((1 << other.gameObject.layer) & player.value) == 0) return;
        if (inputActionReference.action.IsPressed())
        {
            hasPressedButton = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & player.value) != 0)
        {
            textMeshProUGUI.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        }
    }
}




