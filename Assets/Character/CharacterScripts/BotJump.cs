using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotJump : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        
        private void OnEnable()
        {
            botInput.Jump.action.started += JumpActionPress;
            botInput.Jump.action.canceled += OnButtonCancel;
        }

        private void OnDisable()
        {
            botInput.Jump.action.started -= JumpActionPress;
            botInput.Jump.action.canceled -= OnButtonCancel;
        }

        private void JumpActionPress(InputAction.CallbackContext context)
        {
            if (botData.BotStats.NumberOfJump < botData.BotStats.MaxJump && !botData.BotStats.IsDashing && !botData.BotStats.IsCrouching)
            {
                botData.BotStats.NumberOfJump++;
                botData.BotStats.IsJump = true;
                botData.BotStats.HasJumped = true;
                botData.BotDetectionStats.WallDetectionRadius = 0;
            }
        }

        private void FixedUpdate()
        {
            // if (botData.BotStats.HasJumped)
            // {
            //     botData.BotComponents.Rb.velocity =
            //         new Vector2(botData.BotComponents.Rb.velocity.x, Mathf.Max(botData.BotStats.JumpForce, botData.BotStats.InitialJumpForce));
            //     botData.BotStats.HasJumped = false;
            // }
        }

        private void Update()
        {
            if (botData.BotStats.IsJump)
            {
                StartCoroutine(DisableJump());
            }
        }

        private IEnumerator DisableJump()
        {
            yield return new WaitForSecondsRealtime(0.08f);
            botData.BotStats.IsJump = false;
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;

        }

        private void OnButtonCancel(InputAction.CallbackContext context)
        {
            if (botData.BotComponents.Rb.velocity.y > botData.BotStats.InitialJumpForce)
            {
                botData.BotComponents.Rb.velocity = new Vector2(botData.BotComponents.Rb.velocity.x, botData.BotStats.InitialJumpForce);
            }
        }
    }
}