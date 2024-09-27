using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTriggerBox : MonoBehaviour
{
    [SerializeField] private GameObject weaponTransform;
    [SerializeField] private LayerMask pLayerMask;
    [SerializeField] private InputActionReference interactActionReference;
    private bool hasPickedUp;

    public event Action OnWeaponPickedUp;
    public bool HasPickedUp
    {
        get => hasPickedUp;
        set
        {
            hasPickedUp = value;
            if (hasPickedUp)
            {
                OnWeaponPickedUp?.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (HasPickedUp) return;

        if (((1 << other.gameObject.layer) & pLayerMask.value) == 0) return;
        if (!interactActionReference.action.IsPressed()) return;
        weaponTransform.SetActive(false);
        HasPickedUp = true;
    }
}