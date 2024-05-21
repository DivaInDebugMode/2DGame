using System.Collections;
using CombatInterface;
using UnityEngine;

namespace Player.PlayerMovement
{
    public class CharacterGroundedState : CharacterBaseState, IAttacker
    {
        public override void EnterState()
        {
            detectionStats.IsLedgeDetected = false;
            stats.IsJump = false;
            detectionStats.IsClimbing = false;
            stats.IsDashing = false;
            stats.IsAttacking = false;
            stats.NumberOfJumps = 0;
            characterComponents.Rb.gravityScale = 4;
          
        }

        public override void UpdateState()
        {
            MoveAnim();
            Dash();
            Roll();
            

            if (characterInput.Attack.action.triggered && characterInput.MoveUp.action.ReadValue<float>() < 0.9f)
            {
                PerformAttack();
            }
            else if (characterInput.MoveUp.action.ReadValue<float>() >= 0.9f && characterInput.Attack.action.triggered)
            {
                stats.AttackCounter = 4;
                PerformAttack();
            }
        }

        public override void FixedUpdate()
        {
            if (!stats.IsAttacking)
            {
                characterMovement.MoveHorizontally(stats.MoveVector.x * stats.MoveSpeed);
            }
            
            if(stats.IsDashing) characterDash.PerformDash();

        }

        public override void ExitState()
        {
        }

        private void MoveAnim()
        {
            if (stats.IsDashing || stats.IsAttacking) return;

            ctx.StartCoroutine(Land());

            if (stats.IsFalling) return;

            switch (characterComponents.Rb.velocity.x)
            {
                case > 0:
                case < 0:
                    characterAnimation.ChangeAnimationState(characterAnimation.runAnim, 0f);
                    break;
                case 0:
                    characterAnimation.ChangeAnimationState(characterAnimation.idleAnim, 0f);
                    break;
            }
        }

        public void PerformAttack()
        {
            if (stats.IsDashing) return;

            if (stats.AttackCounter == 0 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterComponents.Rb.velocity = Vector2.zero;
                stats.IsAttacking = true;
                characterAnimation.ChangeAnimationState(characterAnimation.groundAttack01, 0.2f);
                stats.AttackCounter++;
            }
            else if (stats.AttackCounter == 1 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterComponents.Rb.velocity = Vector2.zero;
                characterAnimation.ChangeAnimationState(characterAnimation.groundAttack03, 0.2f);
                stats.IsAttacking = true;
                stats.AttackCounter++;
            }
            else if (stats.AttackCounter == 2 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterComponents.Rb.velocity = Vector2.zero;
                var randomValue = Random.Range(0f, 1f);
                characterAnimation.ChangeAnimationState(
                    randomValue < 0.5f ? characterAnimation.punch01Anim : characterAnimation.kick01Anim, 0.2f);
                stats.IsAttacking = true;
                stats.AttackCounter++;
            }
            else if (stats.AttackCounter == 3 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterComponents.Rb.velocity = Vector2.zero;
                characterAnimation.ChangeAnimationState(characterAnimation.groundAttack04, 0.2f);
                stats.IsAttacking = true;
                stats.AttackCounter = 0;
            }
            else if (stats.AttackCounter == 4 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterComponents.Rb.velocity = Vector2.zero;
                characterAnimation.ChangeAnimationState(characterAnimation.groundUpAttack, 0.2f);
                stats.IsAttacking = true;
                stats.AttackCounter = 0;
            }
        }

        private void Roll()
        {
            if (!(characterInput.MoveDown.action.ReadValue<float>() >= 0.8f) ||
                !characterInput.DashAction.action.triggered ||
                !(stats.DashCooldownTimer <= 0) || characterComponents.Rb.velocity.x == 0) return;

            characterDash.Dash();
            if (stats.IsDashing) characterAnimation.ChangeAnimationState(characterAnimation.rollAnim, 0f);
            stats.DashCooldownTimer = 1f;
        }

        private void Dash()
        {
            if (characterInput.MoveDown.action.ReadValue<float>() < 0.8f &&
                characterInput.DashAction.action.triggered &&
                stats.DashCooldownTimer <= 0 && !stats.IsAttacking && !detectionStats.IsWall)
            {
                characterDash.Dash();
                if (stats.IsDashing) characterAnimation.ChangeAnimationState(characterAnimation.dashAnim, 0f);
                stats.DashCooldownTimer = 1f;
            }
        }

        private IEnumerator Land()
        {
            if (!stats.IsFalling) yield break;

            characterAnimation.ChangeAnimationState(characterAnimation.landAnim, 0f);
            yield return new WaitForSeconds(0.1f);
            stats.IsFalling = false;
        }

        public CharacterGroundedState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory,
            CharacterStats characterStats, DetectionStats detectionStats, CharacterDetection characterDetection,
            CharacterComponents components, CharacterInput input, CharacterDash characterDash,
            CharacterMovement characterMovement, CharacterJump characterJump, CharacterAnimation characterAnimation) :
            base(currentContext, characterStateFactory, characterStats, detectionStats, characterDetection, components,
                input, characterDash, characterMovement, characterJump, characterAnimation)
        {
        }
    }
}
