using System;
using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
  
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        public Transform test;
        private void Start()
        {
            botData.BotStats.LastDirectionValue = botData.BotStats.CurrentDirectionValue;
        }

        private void Update()
        {
            HandleBotInput();
            if (botData.BotStats.IsDashing) return;
            if (botData.BotStats.IsWallJump) return;
            if (botData.BotDetectionStats.IsWall) return;
            botData.BotStats.CurrentDirectionValue = botData.BotStats.CurrentDirectionValue switch
            {
                1 when botData.BotStats.MoveDirection.x < 0 => -1,
                -1 when botData.BotStats.MoveDirection.x > 0 => 1,
                _ => botData.BotStats.CurrentDirectionValue
            };
            
            if (!botData.BotStats.IsRotating && botData.BotStats.LastDirectionValue!= botData.BotStats.CurrentDirectionValue)
            {
                botData.BotStats.IsRotating = true;
                botData.BotStats.DirectionTime = 0;
                botData.BotStats.LastDirectionValue = botData.BotStats.CurrentDirectionValue;
            }
            if (botData.BotStats.IsRotating && botData.BotStats.DirectionTime >=0.15f)
            {
                botData.BotStats.IsRotating = false;
            }
        }

        private void FixedUpdate()
        {
            SmoothRotate();
        }

        private void HandleBotInput()
        {
            botData.BotStats.MoveDirection = new Vector2(
                botInput.MoveRight.action.ReadValue<float>() - botInput.MoveLeft.action.ReadValue<float>(),
                0
            ).normalized;
        }

        public void MoveHorizontally(float horizontalSpeed)
        {
            if(botData.BotStats.IsCrouching || botData.BotStats.IsDashing) return;
            if (botData.BotStats.MoveDirection.x != 0)
            {
                botData.BotStats.VelocityX = Mathf.MoveTowards(botData.BotComponents.Rb.velocity.x,
                    botData.BotStats.MoveDirection.x * horizontalSpeed, botData.BotStats.SmoothTime * Time.fixedTime);
                botData.BotComponents.Rb.velocity =
                    new Vector2(botData.BotStats.VelocityX, botData.BotComponents.Rb.velocity.y);
                botData.BotStats.HasStopped = false;

            }
            else if(botData.BotStats.CurrentSpeed <= 4f  && !botData.BotStats.HasStopped)
            {
                botData.BotComponents.Rb.velocity =
                    new Vector2(0, botData.BotComponents.Rb.velocity.y);
                botData.BotStats.HasStopped = true;
            }
        }
        
        private void SmoothRotate()
        {
            if(botData.BotStats.IsDashing) return;
            botData.BotStats.TargetAngle = botData.BotStats.CurrentDirectionValue switch
            {
                > 0 when Math.Abs(botData.BotStats.TargetAngle - 90f) > 0.00001f => 90,
                < 0 when Math.Abs(botData.BotStats.TargetAngle - 270) > 0.00001f => 270f,
                _ => botData.BotStats.TargetAngle
            };

            if (!(Math.Abs(transform.eulerAngles.y - botData.BotStats.TargetAngle) > 0.00001f)) return;
            botData.BotStats.DirectionTime += Time.deltaTime;
            var targetRotation = Quaternion.Euler(0, botData.BotStats.TargetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 25);
        }
        private void StandingFromCrouch() => botData.BotStats.IsCrouching = false;
        private void Test() =>  botData.BotComponents.MoveCollider.transform.position = test.transform.position;
    }
}