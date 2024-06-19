using System;
using System.Collections;
using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
  
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private BotComponents botComponents;
        [SerializeField] private BotStats botStats;
        [SerializeField] private BotInput botInput;


        private void Start()
        {
            botStats.LastDirectionValue = botStats.CurrentDirectionValue;
        }

        private void Update()
        {
            HandleBotInput();
            botStats.CurrentDirectionValue = botStats.CurrentDirectionValue switch
            {
                1 when botStats.MoveDirection.x < 0 => -1,
                -1 when botStats.MoveDirection.x > 0 => 1,
                _ => botStats.CurrentDirectionValue
            };
            
            if (!botStats.IsRotating && botStats.LastDirectionValue!= botStats.CurrentDirectionValue)
            {
                botStats.IsRotating = true;
                botStats.DirectionTime = 0;
                botStats.LastDirectionValue = botStats.CurrentDirectionValue;
            }
            if (botStats.IsRotating && botStats.DirectionTime >=0.1f)
            {
                botStats.IsRotating = false;
            }
        }

        private void FixedUpdate()
        {
            SmoothRotate();
        }

        private void HandleBotInput()
        {
            botStats.MoveDirection = new Vector2(
                botInput.MoveRight.action.ReadValue<float>() - botInput.MoveLeft.action.ReadValue<float>(),
                0
            ).normalized;
        }

        public void MoveHorizontally(float horizontalSpeed)
        {
            if(botStats.IsCrouching || botStats.IsDashing) return;
            if (botStats.MoveDirection.x != 0)
            {
                botStats.VelocityX = Mathf.MoveTowards(botComponents.Rb.velocity.x,
                    botStats.MoveDirection.x * horizontalSpeed, botStats.SmoothTime * Time.fixedTime);
                botComponents.Rb.velocity =
                    new Vector2(botStats.VelocityX, botComponents.Rb.velocity.y);
                botStats.HasStopped = false;

            }
            else if(botStats.CurrentSpeed <= 4f  && !botStats.HasStopped)
            {
                botComponents.Rb.velocity =
                    new Vector2(0, botComponents.Rb.velocity.y);
                botStats.IsRunning = false;
                botStats.HasStopped = true;
            }
        }
        
        private void SmoothRotate()
        {
            if (botStats.IsDashing) return;
            botStats.TargetAngle = botStats.CurrentDirectionValue switch
            {
                > 0 when Math.Abs(botStats.TargetAngle - 90f) > 0.00001f => 90,
                < 0 when Math.Abs(botStats.TargetAngle - 270) > 0.00001f => 270f,
                _ => botStats.TargetAngle
            };

            if (!(Math.Abs(transform.eulerAngles.y - botStats.TargetAngle) > 0.00001f)) return;
            botStats.DirectionTime += Time.deltaTime;
            var targetRotation = Quaternion.Euler(0, botStats.TargetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15);

        }
        
        
        private void TurnOfEdgeFall() => botStats.IsFallingEdge = false;
        private void StandingFromCrouch() => botStats.IsCrouching = false;

    }
}
