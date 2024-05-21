using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class KeyboardManager : MonoBehaviour
    { 
        public PlayerInput playerInput;
        private static InputActionRebindingExtensions.RebindingOperation _rebinding;
        private string sameAction;
        public event Action<int, string> OnInputChange;
        public event Action<int> OnKeyOnRebindStart, OnKeOnRebindComplete, OnSameKey;
        public event Action OnKeyReset;
        private const string JumpAction = "Jump";
        private const string AttackAction = "Attack";
        private const string RangeAction = "Range";
        private const string HealAction = "Heal";
        private const string DashAction = "Dash";
        private const string InteractAction = "Interact";
        private const string MapAction = "Map";
        private const string InventoryAction = "Inventory";
        private const string UpAction = "Up";
        private const string DownAction = "Down";
        private const string LeftAction = "Left";
        private const string RightAction = "Right";
        [SerializeField] private InputOverridePath inputOverridePath;
        [SerializeField] private RebindCondition rebindCondition;

        private readonly Dictionary<string, string> defaultKeys = new()
        {
            { UpAction, "<Keyboard>/upArrow" },
            { DownAction, "<Keyboard>/downArrow" },
            { LeftAction, "<Keyboard>/leftArrow" },
            { RightAction, "<Keyboard>/rightArrow" },
            { JumpAction, "<Keyboard>/space" },
            { AttackAction, "<Keyboard>/q" },
            { RangeAction, "<Keyboard>/w" },
            { DashAction, "<Keyboard>/leftShift" },
            { InteractAction, "<Keyboard>/e" },
            { MapAction, "<Keyboard>/m" },
            { InventoryAction, "<Keyboard>/i" },
            { HealAction, "<Keyboard>/r" },
        };

        private string filePath;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            filePath = Path.Combine(Application.dataPath, "SaveFiles/keyboardInputs.json");

            for (var i = 0; i < inputOverridePath.KeyboardInputs.Count  ; i++) {
                playerInput.actions.actionMaps[0].actions[i]
                    .ApplyBindingOverride(0, new InputBinding { overridePath = inputOverridePath.KeyboardInputs[i] });
            }
            LoadRebinds();
        }
        
        public void StartRebinding(int index)
        {
            rebindCondition.IsRebinding = true;
            OnKeyOnRebindStart?.Invoke(index);
            foreach (var action in playerInput.actions.actionMaps[0].actions) action.Disable();
            _rebinding = playerInput.actions.actionMaps[0].actions[index].PerformInteractiveRebinding(0)
                .WithControlsExcluding("<Keyboard>/enter")
                .WithControlsExcluding("<Keyboard>/escape")
                .WithControlsExcluding("<Keyboard>/anyKey") 
                .WithControlsExcluding("<Keyboard>/numpadEnter")
                .WithControlsExcluding("<Keyboard>/capsLock")
                .WithControlsExcluding("<Keyboard>/contextMenu")
                .WithControlsExcluding("<Keyboard>/leftMeta")
                .WithControlsExcluding("<Gamepad>/");
                
            _rebinding.OnComplete(_ => RebindComplete(index));
            _rebinding.Start();
        }
        private void RebindComplete(int index)
        {
            foreach (var action in playerInput.actions.actionMaps[0].actions) action.Enable();
            OnKeOnRebindComplete?.Invoke(index);
            CheckSameKey(index);
            OnInputChange?.Invoke(index,
                playerInput.actions.actionMaps[0].actions[index].bindings[0].effectivePath);
            inputOverridePath.KeyboardInputs[index] = playerInput.actions.actionMaps[0].actions[index].bindings[0].effectivePath;
            _rebinding.Dispose();
            StartCoroutine(ResetCondition());
            SaveRebinds();
        }

        private void CheckSameKey(int index)
        {
            var actionMap = playerInput.actions.actionMaps[0];

            for (var i = 0; i < actionMap.actions.Count -2; i++)
            {
                if (actionMap.actions[i].bindings[0].effectivePath !=
                    actionMap.actions[index].bindings[0].effectivePath || i == index) continue;
                sameAction = i switch
                {
                    0 => UpAction,
                    1 => DownAction,
                    2 => LeftAction,
                    3 => RightAction,
                    4 => JumpAction,
                    5 => AttackAction,
                    6 => RangeAction,
                    7 => HealAction,
                    8 => DashAction,
                    9 => InteractAction,
                    10 => MapAction,
                    11 => InventoryAction,

                    _ => sameAction
                };
                playerInput.actions[sameAction]
                        
                    .ApplyBindingOverride(0,new InputBinding { overridePath = string.Empty }
                    );
                OnSameKey?.Invoke(i);
                inputOverridePath.KeyboardInputs[i] = string.Empty;
            }
        }

        public void SetKeyDefault()
        {
            foreach (var kvp in defaultKeys) playerInput.actions[kvp.Key].ApplyBindingOverride
                (new InputBinding { overridePath = kvp.Value });
           
            for (var index = 0; index < 12; index++) inputOverridePath.KeyboardInputs[index] =
                playerInput.actions.actionMaps[0].actions[index].bindings[0].overridePath;
            
            OnKeyReset?.Invoke();
            SaveRebinds();
        }
        
        private IEnumerator ResetCondition()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            rebindCondition.IsRebinding = false;
        }

        private void SaveRebinds()
        {
            var saveRebindsJson = playerInput.actions.SaveBindingOverridesAsJson();
            File.WriteAllText(filePath, saveRebindsJson);
        }

        private void LoadRebinds()
        {
            if (File.Exists(filePath))
            {
                var loadRebindsJson = File.ReadAllText(filePath);
                playerInput.actions.LoadBindingOverridesFromJson(loadRebindsJson);
            }
            else
            {
                Debug.LogWarning("No saved rebinds file found.");
            }
        }
    }
}