using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotJump : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        private float maxHeight;
        
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
            if (botData.BotDetectionStats.IsGrounded && !botData.BotStats.IsDashing &&
                !botData.BotStats.IsCrouching && botData.BotStats.CanGroundJump && !isJumping)
            {
                botData.BotStats.IsJump = true;
                botData.BotStats.HasJumped = true;
                botData.BotStats.CanGroundJump = false;
                botData.BotDetectionStats.WallDetectionRadius = 0;
            }
        }

        private void FixedUpdate()
        {
            if (!botData.BotStats.HasJumped) return;
            botData.BotComponents.Rb.velocity =
                new Vector2(botData.BotComponents.Rb.velocity.x, Mathf.Max(botData.BotStats.JumpForce, botData.BotStats.InitialJumpForce));
            botData.BotStats.HasJumped = false;
        }

        private float jumpTimer = 0f;
        private bool isJumping = false;
        private float jumpDuration = 0.08f;

        private void Update()
        {
            if (botData.BotStats.IsJump)
            {
                isJumping = true;
                jumpTimer = jumpDuration;
                botData.BotStats.IsJump = false;
            }

            if (isJumping)
            {
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0f)
                {
                    isJumping = false;
                    botData.BotDetectionStats.WallDetectionRadius = 0.5f;
                }
            }
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