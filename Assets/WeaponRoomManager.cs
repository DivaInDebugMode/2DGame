using System;
using UnityEngine;

public class WeaponRoomManager : MonoBehaviour
{
    [SerializeField] private WeaponTriggerBox weaponTriggerBox;
    [SerializeField] private TutorialTextBoxes attackTutorialBox;
    [SerializeField] private MovingWalls weaponRoomWall;
    [SerializeField] private MovingWalls enemyEnterWall;

    private void Start()
    {
        attackTutorialBox.enabled = false;
        weaponRoomWall.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        weaponTriggerBox.OnWeaponPickedUp += HandleWeaponPickedUp;
        attackTutorialBox.OnButtonPressed += HandleButtonPressed;
        enemyEnterWall.OnWallStopped += RestartWeaponRoomWall;
        weaponRoomWall.OnWallStopped += RestartEnemyRoomWall;
    }

    private void OnDestroy()
    {
        weaponTriggerBox.OnWeaponPickedUp -= HandleWeaponPickedUp;
        attackTutorialBox.OnButtonPressed -= HandleButtonPressed;
        enemyEnterWall.OnWallStopped -= RestartWeaponRoomWall;
        weaponRoomWall.OnWallStopped -= RestartEnemyRoomWall;

    }

    private void HandleWeaponPickedUp()
    {
        attackTutorialBox.enabled = true;
    }

    private void HandleButtonPressed()
    {
        weaponRoomWall.gameObject.SetActive(true);
    }

    private void RestartWeaponRoomWall()
    {
        weaponRoomWall.HasOpened = false;
    }
    
    private void RestartEnemyRoomWall()
    {
        enemyEnterWall.HasOpened = false;
    }
    
}