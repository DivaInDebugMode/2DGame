using System.Collections;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotIceState : BotBaseState
    {
        public BotIceState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botInput, botData, botAnimatorController)
        {
            
        }

        private float currentSpeed;
        private const float MaxSpeed = 6f;
        private static readonly int SlideRun = Animator.StringToHash("SlideRun");
        private static readonly int SlideIdle = Animator.StringToHash("SlideIdle");
        [Header("Jump Timer Variables for enable jump action")]
        private bool jumpTimerOn;
        private float jumpStartTimer;
        private float jumpTimerDuration;
        private bool test;

        public override void EnterState()
        {
            // test = true;
            Physics.gravity = botData.BotStats.GroundGForce;
            // switch (botData.BotStats.CurrentDirectionValue)
            // {
            //     case 1:
            //         botData.BotComponents.Rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
            //         break;
            //     case -1:
            //         botData.BotComponents.Rb.velocity = new Vector2(-5f, 0f);
            //         break;
            // }
            jumpStartTimer = Time.time;
            jumpTimerOn = true;
            botData.BotStats.IsWallJump = false;
            botData.BotDetectionStats.IsWall = false;
          
        }

        public override void UpdateState()
        {
          HandleIceSpeed();
          HandleIceAnimations();
          JumpActionResetTimer();
          // if (botInput.MoveLeft.action.IsPressed() | botInput.MoveRight.action.IsPressed())
          // { 
          //     ctx.StartCoroutine(Timer());
          // }
          //

        }

        public override void FixedUpdate()
        {
           IceMovement(currentSpeed);
           
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(SlideRun,false);
            botAnimatorController.Animator.SetBool(SlideIdle,false);
            botData.BotComponents.Rb.velocity = Vector3.zero;
          
        }

        private IEnumerator Timer()
        {
            if (test)
                yield return new WaitForSecondsRealtime(0.5f);
            test = false;
        }
        private void IceMovement(float horizontalSpeed)
        {
           // if(test)return;
            if (botData.BotStats.IsCrouching || botData.BotStats.IsGroundDashing) return;

            // თუ მოძრაობის მიმართულება != 0, მივანიჭოთ ველოსიტი
            if (botData.BotStats.MoveDirection.x != 0)
            {
                botData.BotComponents.Rb.velocity = new Vector2(
                    botData.BotStats.MoveDirection.x * horizontalSpeed, 
                    botData.BotComponents.Rb.velocity.y
                );
            }
            // თუ მოძრაობა შეჩერდა, სიჩქარის შედარებით და ველოსიტის შემოწმებით
            else if (horizontalSpeed > 0)
            {
                // შევინარჩუნოთ არსებული ველოსიტი სანამ სიჩქარე 0-ს არ მიუახლოვდება
                botData.BotComponents.Rb.velocity = new Vector2(
                    Mathf.Sign(botData.BotComponents.Rb.velocity.x) * horizontalSpeed,
                    botData.BotComponents.Rb.velocity.y
                );
            }
            // მხოლოდ მაშინ შევაჩეროთ, როცა სიჩქარე პრაქტიკულად ნულია
            else
            {
                botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
            }
        }



        private void HandleIceSpeed()
        {
            // სიჩქარის ზრდის ტემპი, რომ 2.5 წამში მიაღწიოს მაქსიმუმს
            float accelerationRate = MaxSpeed / 2.5f;
            // სიჩქარის შემცირების ტემპი, რომ 2.5 წამში მივიდეს ნულამდე
            float decelerationRate = MaxSpeed / 2.5f;

            // თუ სიჩქარე ნაკლებია მაქსიმალურზე და მოძრაობა ხდება, ვზრდით სიჩქარეს
            if (botData.BotStats.MoveDirection.x != 0)
            {
                currentSpeed += accelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, MaxSpeed); // ლიმიტი მაქსიმალური სიჩქარისთვის
            }
            // თუ მოძრაობა შეჩერდა, სიჩქარე თანდათანობით მცირდება ნულამდე
            else if (botData.BotStats.MoveDirection.x == 0 && currentSpeed > 0)
            {
                currentSpeed -= decelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f); // არ გადააჭარბოს მინიმუმს
            }
            
        }

        private void HandleIceAnimations()
        {
            if (botData.BotStats.MoveDirection.x == 0)
            {
               botAnimatorController.Animator.SetBool(SlideIdle,true);
               botAnimatorController.Animator.SetBool(SlideRun,false);
            }
            else if (botData.BotStats.MoveDirection.x != 0)
            {
                botAnimatorController.Animator.SetBool(SlideRun,true);
                botAnimatorController.Animator.SetBool(SlideIdle,false);
            }
        }
        
        private void JumpActionResetTimer()
        {
            if (jumpTimerOn)
            {
                jumpTimerDuration = Time.time - jumpStartTimer;
                if (jumpTimerDuration >= 0.1f)
                {
                    botData.BotStats.HasJumped = false;
                    jumpTimerOn = false;
                }
            }
        }
        
    }
}