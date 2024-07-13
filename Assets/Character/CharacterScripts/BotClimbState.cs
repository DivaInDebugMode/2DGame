using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");

       

        public override void EnterState()
        {
            Physics.gravity = new Vector2(0.0f, -3.5f);
            botData.BotComponents.Rb.velocity = Vector3.zero;
            botAnimatorController.Animator.SetBool(WallSlide, true);
        }

        public override void UpdateState()
        {
            if (Math.Abs(botData.BotStats.TargetAngle - 270f) < 0.0001f && botInput.MoveRight.action.triggered)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0f;
            }
        }

        public override void FixedUpdate()
        {
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(WallSlide, false);
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;
        }

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}