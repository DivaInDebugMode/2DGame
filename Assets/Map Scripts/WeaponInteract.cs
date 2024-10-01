using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInteract : MonoBehaviour
{
    [SerializeField] private GameObject weaponTransform;
    [SerializeField] private GameObject handWeaponTransform;
    [SerializeField] private GameObject secondWeaponTransform;
    [SerializeField] private LayerMask weaponTrigger;
    [SerializeField] private InputActionReference interactActionReference;
    [SerializeField] private bool hasPickedUp;

    private void SwordEquip()
    {
        weaponTransform.SetActive(false);
        handWeaponTransform.SetActive(true);
    }

    private void SwordUnEquip()
    {
        weaponTransform.SetActive(true);
        handWeaponTransform.SetActive(false);
    }

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