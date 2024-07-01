using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int Property = Animator.StringToHash("Wall Slide");

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotJump botJump) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botJump)
        {
        }

        public override void EnterState()
        {
            //botData.BotStats.NumberOfJump = 0;
            Physics.gravity = new Vector2(0.0f, -3.5f);
            botData.BotComponents.Rb.velocity = Vector3.zero;
            botAnimatorController.Animator.SetBool(Property, true);
        }

        public override void UpdateState()
        {
            if (Math.Abs(botData.BotStats.TargetAngle - 270f) < 0.0001f && botInput.MoveRight.action.triggered)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0f;            }
        }

        private void FreezeCharacterOnLedge()
        {
            botData.BotComponents.Rb.useGravity = false;
            botData.BotComponents.Rb.velocity = Vector3.zero;
        }

        public override void FixedUpdate()
        {
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Property, false);
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;
        }
    }
}