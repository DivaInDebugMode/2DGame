using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInteract : MonoBehaviour
{
    [SerializeField] private GameObject weaponTransform;
    [SerializeField] private LayerMask weaponTrigger;
    [SerializeField] private InputActionReference interactActionReference;
    [SerializeField] private bool hasPickedUp;

    private void OnTriggerStay(Collider other)
    {
        if(hasPickedUp) return;
        if (((1 << other.gameObject.layer) & weaponTrigger.value) != 0)
        {
            if (interactActionReference.action.IsPressed())
            {
                weaponTransform.SetActive(true);
                hasPickedUp = true;
            }
        }
    }
}