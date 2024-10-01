using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotJump : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        private bool isPressed;
        private float pressStartTime;
        private float jumpPressedTime;
        private bool isTap;
        private float dropTimer;
        private bool shouldDrop;
        private void OnEnable()
        {
            botInput.Jump.action.started += JumpActionPress;
            botInput.Jump.action.canceled += OnButtonCancel;
        }

        private void OnDisable()
        {
            botInput.Jump.action.started -= JumpActionPress;
            botInput.Jump.action.canceled -= OnButtonCancel;
        }
        private void JumpActionPress(InputAction.CallbackContext context)
        {
            if (botData.BotDetectionStats.IsGrounded &&
                !botData.BotStats.IsCrouching && !botData.BotStats.HasJumped)
            {
                botData.BotStats.HasJumped = true;
                botData.BotStats.IsJump = true;
                pressStartTime = Time.unscaledTime; // Use unscaled time to prevent frame rate issues
                isPressed = true;
                isTap = false;
                shouldDrop = false;
                dropTimer = 0f;
                switch (botData.BotStats.MoveDirection.x)
                {
                    case 0:
                        botData.Rb.velocity = new Vector2(0, botData.BotStats.JumpForce);
                        break;
                    default:
                    {
                        if (botData.BotStats.MoveDirection.x != 0)
                            botData.Rb.velocity = new Vector2(botData.Rb.velocity.x, botData.BotStats.JumpForce);
                        break;
                    }
                }
            }
            else if (botData.BotDetectionStats.IsWall && !botData.BotStats.IsWallJump)
            {
                botData.BotStats.HasWallJumped = false;
                botData.BotStats.IsWallJump = true;
                Physics.gravity = botData.BotStats.WallGForce;
                botData.BotStats.WallJumpDurationStart = true;
            }
        }

        private void FixedUpdate()
        {
            JumpPhysicsFromWall();
            if (botData.BotStats.IsAirDashing) return;

            if (isPressed)
            {
                jumpPressedTime = Time.unscaledTime - pressStartTime; // Use unscaled time for accuracy
                isTap = jumpPressedTime <= 0.15f; // Check if it's a tap
            }

            if (!isTap || shouldDrop) return;

            dropTimer += Time.fixedDeltaTime;
            if (!(dropTimer >= 0.17f)) return;
            switch (botData.BotStats.MoveDirection.x)
            {
                case 0:
                    botData.Rb.velocity = new Vector2(0, botData.BotStats.InitialJumpForce);
                    break;
                default:
                    if (botData.BotStats.MoveDirection.x != 0)
                    {
                        botData.Rb.velocity = new Vector2(botData.Rb.velocity.x, botData.BotStats.InitialJumpForce);
                    }

                    break;
            }

            shouldDrop = true;
        }

        private void JumpPhysicsFromWall()
        {
            if (botData.BotStats.IsWallJump && !botData.BotStats.HasWallJumped)
            {
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case 1:
                        botData.Rb.velocity = new Vector2(-botData.BotStats.WallJumpForce.x,
                            botData.BotStats.WallJumpForce.y);
                        botData.BotStats.TargetAngle = 270f;
                        botData.BotStats.CurrentDirectionValue = -1;
                        botData.BotStats.LastDirectionValue = -1;
                        botData.BotStats.HasWallJumped = true;
                        break;
                    case -1:
                        botData.Rb.velocity = botData.BotStats.WallJumpForce;
                        botData.BotStats.TargetAngle = 90f;
                        botData.BotStats.CurrentDirectionValue = 1;
                        botData.BotStats.LastDirectionValue = 1;
                        botData.BotStats.HasWallJumped = true;
                        break;
                }
            }
        }

        private void OnButtonCancel(InputAction.CallbackContext context)
        {
            if (botData.BotDetectionStats.IsWall) return;

            isPressed = false;
            if (!(jumpPressedTime > 0.15f)) return;
            isTap = false;
            dropTimer = 0f;
            shouldDrop = false;
        }
    }
}
