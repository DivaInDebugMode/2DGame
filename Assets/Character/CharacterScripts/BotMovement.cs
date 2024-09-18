using System;
using System.Collections;
using UnityEngine;

namespace Character.CharacterScripts
{
  
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        private void Start()
        {
            botData.BotStats.LastDirectionValue = botData.BotStats.CurrentDirectionValue;
        }

        private void Update()
        {
            HandleBotInput();
            BackTransformToZ();
            PlayerDirectionSet();
        }

        private void FixedUpdate()
        {
            SmoothRotate();
            BounceMovement();
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
            if(botData.BotStats.IsHurricaneBounce || botData.BotStats.IsMegaBounce) return;
            if (botData.BotStats.IsCrouching || botData.BotStats.IsGroundDashing) return;
            if(botData.BotDetectionStats.IsOnEdge) return;
            if (botData.BotStats.MoveDirection.x != 0)
            {
                botData.BotComponents.Rb.velocity = new Vector2(
                    botData.BotStats.MoveDirection.x * horizontalSpeed, 
                    botData.BotComponents.Rb.velocity.y);
                botData.BotStats.HasStopped = false;
            }
            else if (botData.BotStats.CurrentSpeed <= 4f && !botData.BotStats.HasStopped)
            {
                botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
                botData.BotStats.HasStopped = true;
            }
        }
        
        #region BounceActions

        private void BounceMovement()
        {
            if (botData.BotStats.IsHurricaneBounce)
            {
                botData.BotStats.CanAirDash = false;
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case -1:
                        if (!botData.BotStats.HasHurricaned)
                        {
                            botData.BotComponents.Rb.velocity = new Vector2(-15,15);
                            StartCoroutine(HurricaneTimer());
                            botData.BotStats.HasHurricaned = true;
                        }
                        break;
                    case 1:
                        if (!botData.BotStats.HasHurricaned)
                        {
                            botData.BotComponents.Rb.velocity = new Vector2(15,15);
                            StartCoroutine(HurricaneTimer());
                            botData.BotStats.HasHurricaned = true;
                        }                      
                        break;
                }

            }else if (botData.BotStats.IsMegaBounce)
            {
                botData.BotComponents.Rb.velocity = new Vector2(botData.BotComponents.Rb.velocity.x,22);
                StartCoroutine(BounceTimer());
            }
        }
        private IEnumerator HurricaneTimer()
        {
            yield return new WaitForSecondsRealtime(1f);
            botData.BotStats.CanAirDash = true;
            botData.BotStats.CanAirDashAnimation = true;
            botData.BotStats.HasHurricaned = false;
            botData.BotStats.IsHurricaneBounce = false;
            botData.BotStats.StopGlide = false;
        }

        private IEnumerator BounceTimer()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            botData.BotStats.IsMegaBounce = false;
            botData.BotStats.CanAirDash = true;
            botData.BotStats.CanAirDashAnimation = true;
            botData.BotStats.StopGlide = false;
        }

        #endregion
        
        private void SmoothRotate()
        {
            if(botData.BotStats.IsGroundDashing || botData.BotStats.IsAirDashing) return;
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
        
        private void PlayerDirectionSet()
        {
            if (botData.BotStats.IsGroundDashing) return;
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
        
        private void BackTransformToZ()
        {
            var transformPosition = transform.position;
            if (!(Mathf.Abs(transformPosition.z) > 0.0001f) && !(Mathf.Abs(transformPosition.z) < -0.0001f)) return;
            transformPosition.z = 0;
            transform.position = transformPosition;
        }
        
        private void StandingFromCrouch() => botData.BotStats.IsCrouching = false;
        
        
        // public void MoveHorizontally(float horizontalSpeed)
        // {
        //    
        //     if(botData.BotStats.IsCrouching || botData.BotStats.IsGroundDashing) return;
        //     if (botData.BotStats.MoveDirection.x != 0)
        //     {
        //         botData.BotStats.VelocityX = Mathf.MoveTowards(botData.BotComponents.Rb.velocity.x,
        //             botData.BotStats.MoveDirection.x * horizontalSpeed, botData.BotStats.SmoothTime * Time.fixedTime);
        //         botData.BotComponents.Rb.velocity =
        //             new Vector2(botData.BotStats.VelocityX, botData.BotComponents.Rb.velocity.y);
        //         botData.BotStats.HasStopped = false;
        //
        //     }
        //     else if(botData.BotStats.CurrentSpeed <= 4f  && !botData.BotStats.HasStopped)
        //     {
        //         botData.BotComponents.Rb.velocity =
        //             new Vector2(0, botData.BotComponents.Rb.velocity.y);
        //         botData.BotStats.HasStopped = true;
        //       
        //     }
        // }
        //
    }
}