using System.Collections;
using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public enum Directions
    {
        Right,
        Left
    }
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private BotComponents botComponents;
        [SerializeField] private BotStats botStats;
        [SerializeField] private BotInput botInput;
        
        private void Start()
        {
            botStats.CurrentDirection = Directions.Right;
        }

        private void Update()
        {
            HandleBotInput();
        }

        private void FixedUpdate()
        {
            GroundRotateBot();
        }

        private void HandleBotInput()
        {
            if(botStats.IsCrouching) return;
            botStats.MoveDirection = new Vector2(
                botInput.MoveRight.action.ReadValue<float>() - botInput.MoveLeft.action.ReadValue<float>(),
                0
            ).normalized;
        }

        public void MoveHorizontally(float horizontalSpeed)
        {
            if(botStats.IsRotating || botStats.IsCrouching) return;
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

        private void GroundRotateBot()
        {
            if (botStats.IsRotating || botStats.IsCrouching) return;
            switch (botStats.MoveDirection.x)
                {
                    case > 0:
                        if (botStats.CurrentDirection == Directions.Right) return;
                        botStats.CurrentDirectionValue = 1;
                        botStats.IsRotating = true;
                        var velocity1 = botComponents.Rb.velocity;
                        if (botStats.CurrentSpeed > 3.5f && botStats.DirectionTime >= 0.5f)
                        {
                            velocity1 = new Vector2(-1.5f, velocity1.y);
                        }
                        else if (botStats.CurrentSpeed < 3.5f || botStats.DirectionTime < 0.5f)
                        {
                            velocity1 = new Vector2(0f, velocity1.y);
                        }

                        botComponents.Rb.velocity = velocity1;
                        transform.rotation = Quaternion.Euler(0, 90, 0);
                        StartCoroutine(DirectionTimer());
                        botStats.CurrentDirection = Directions.Right;
                        break;
                    case < 0:
                        if (botStats.CurrentDirection == Directions.Left) return;
                        botStats.IsRotating = true;
                        botStats.CurrentDirectionValue = -1;
                        var velocity = botComponents.Rb.velocity;
                        if (botStats.CurrentSpeed > 3.5f && botStats.DirectionTime >= 0.5f)
                        {
                            velocity = new Vector2(1.5f, velocity.y);
                        }
                        else if (botStats.CurrentSpeed <= 3.5f || botStats.DirectionTime < 0.5f)
                        {
                            velocity = new Vector2(0f, velocity.y);
                        }
                        botComponents.Rb.velocity = velocity;
                        transform.rotation = Quaternion.Euler(0, -90, 0);
                        StartCoroutine(DirectionTimer());
                        botStats.CurrentDirection = Directions.Left;
                        break;
                }
        }

        private IEnumerator DirectionTimer()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            botStats.DirectionTime = 0;
        }

        private void RunToStop() => botStats.IsRunning = false;
        private void RotateToRun() => botStats.IsRotating = false;
        private void Rotated() => botStats.HasRotate = false;
        private void TurnOfEdgeFall() => botStats.IsFallingEdge = false;

        private void StandingFromCrouch() => botStats.IsCrouching = false;

    }
}
