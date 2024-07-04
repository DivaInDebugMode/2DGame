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
                    EndDash();
                }
            }

            if (botData.BotStats.CanDash) return;
            botData.BotStats.DashTimer += Time.deltaTime;
            if (!(botData.BotStats.DashTimer >= botData.BotStats.DashCooldown)) return;
            botData.BotStats.CanDash = true;
            botData.BotStats.DashTimer = 0;
        }

        private void Dash(InputAction.CallbackContext context)
        {
            if (botData.BotStats.IsCrouching) return;
            if (botData.BotStats.IsRotating) return;
            if(botData.BotStats.IsGliding) return;
            if (!botData.BotStats.CanDash) return;
            botData.BotStats.IsDashing = true;
            botData.BotStats.CurrentSpeed = 0;
            botData.BotStats.DashCooldownStart = 0;

           

            botData.BotStats.HasDashed = true;
            botData.BotStats.CanDash = false;
        }

        private void EndDash()
        {
            botData.BotStats.IsDashing = false;
            botData.BotStats.DirectionTime = 0;
            botData.BotStats.DashCooldownStart = 0;
            botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
        }

        public void Dash()
        {
            var velocity = botData.BotComponents.Rb.velocity;
            velocity = botData.BotStats.CurrentDirectionValue switch
            {
                1 => new Vector2(botData.BotStats.DashForce, 0),
                -1 => new Vector2(-botData.BotStats.DashForce, 0),
                _ => velocity
            };
            botData.BotComponents.Rb.velocity = velocity;
        }
    }
}
