using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotDash : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;

        private void OnEnable()
        {
            botInput.Dash.action.started += Dash;
        }

        private void OnDisable()
        {
            botInput.Dash.action.started -= Dash;
        }

        private void Update()
        {
            
            if (botData.BotStats.IsDashing)
            {
                botData.BotStats.DashCooldownStart += Time.deltaTime;
                if (botData.BotStats.DashCooldownStart > botData.BotStats.DashDuration)
                {
                    botData.BotStats.IsDashing = false;
                    botData.BotStats.DirectionTime = 0;
                    botData.BotStats.DashCooldownStart = 0;
                }
            }

            if (!botData.BotStats.CanDash)
            {
                botData.BotStats.DashTimer += Time.deltaTime;
                if (botData.BotStats.DashTimer >= botData.BotStats.DashCooldown)
                {
                    botData.BotStats.CanDash = true;
                    botData.BotStats.DashTimer = 0;
                }
            }
        }

        private void Dash(InputAction.CallbackContext context)
        {
            if(!botData.BotStats.CanDash) return;
            if(botData.BotStats.IsRotating) return;
            botData.BotStats.IsDashing = true;
            switch (botData.BotStats.MoveDirection.x)
            {
                case 1:
                    botData.BotComponents.Rb.velocity = new Vector2(botData.BotStats.DashForce, botData.BotComponents.Rb.velocity.y);
                    break;
                case -1:
                    botData.BotComponents.Rb.velocity = new Vector2(-botData.BotStats.DashForce, botData.BotComponents.Rb.velocity.y);
                    break;
                    
            }

            botData.BotStats.HasDashed = true;
            botData.BotStats.CanDash = false;
        }
        
    }
}