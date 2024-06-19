
using System;
using Character.CharacterScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScriptable
{
    public class BotJump : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        private bool isPressed;
        private bool isLongJump;
       [SerializeField] private float pressStartTime;
       [SerializeField] private float jumpPressedTime;

        private void OnEnable()
        {
            botInput.Jump.action.started += JumpActionPressed;
            botInput.Jump.action.canceled += OnJumpedActionCanceled;
        }

        private void OnDisable()
        {
            botInput.Jump.action.started -= JumpActionPressed;
            botInput.Jump.action.canceled -= OnJumpedActionCanceled;
        }

        private void Update()
        {
            if(!isPressed)return;
            jumpPressedTime = Time.time - pressStartTime;
            if (jumpPressedTime <= 0.20)
            {
                botData.BotComponents.Rb.velocity = 
                    new Vector2(botData.BotComponents.Rb.velocity.x, botData.BotStats.JumpForce);
               
                
            }
          
        }

        private void JumpActionPressed(InputAction.CallbackContext context)
        {
           
            if (botData.BotStats.NumberOfJump < botData.BotStats.MaxJump && !botData.BotStats.IsDashing)
            {
                pressStartTime = Time.time;
                botData.BotStats.NumberOfJump++;
                botData.BotStats.IsJump = true;
                botData.BotStats.EndedJumpEarly = false;
                isPressed = true;
               
               
            }
        }
        
        private void OnJumpedActionCanceled(InputAction.CallbackContext context)
        {
            isPressed = false;
           
        }

        public void Jump()
        {
            if (!botData.BotStats.IsJump) return;
            isPressed = true;
            botData.BotStats.IsJump = false;
            
        }
    }
}