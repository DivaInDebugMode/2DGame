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
        [SerializeField] private BotAnimatorController botAnimatorController;

        private void Start()
        {
            botStats.CurrentDirection = Directions.Right;
        }

        private void Update()
        {
            HandleBotInput();
            if (botStats.MoveDirection.x == 0)
            {
                botAnimatorController.TurnOfRoot();
            }
        }

        private void FixedUpdate()
        {
            RotateBot();
            MoveHorizontally(botStats.MoveSpeed);
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
            if(botStats.IsRotating) return;
            if (botStats.MoveDirection.x != 0)
            {
                botStats.VelocityX = Mathf.MoveTowards(botComponents.Rb.velocity.x,
                    botStats.MoveDirection.x * horizontalSpeed, botStats.SmoothTime * Time.fixedTime);
                botComponents.Rb.velocity =
                    new Vector2(botStats.VelocityX, botComponents.Rb.velocity.y);
            }
            else if(botStats.MoveDirection.x == 0)
            {
                botComponents.Rb.velocity =
                    new Vector2(0, botComponents.Rb.velocity.y);
               
            }
        }

        private void RotateBot()
        {
            switch (botStats.MoveDirection.x)
                {
                    case 1:
                        if (botStats.CurrentDirection == Directions.Right) return;
                        botStats.IsRotating = true;
                        botStats.IsRunning = false;
                        transform.rotation = Quaternion.Euler(0, 90, 0);
                        botAnimatorController.TurnOnRoot();
                        botStats.CurrentDirection = Directions.Right;
                        break;
                    case -1:
                        if (botStats.CurrentDirection == Directions.Left) return;
                        botStats.IsRotating = true;
                        botStats.IsRunning = false;
                        transform.rotation = Quaternion.Euler(0, -90, 0);
                        botAnimatorController.TurnOnRoot();
                        botStats.CurrentDirection = Directions.Left;
                        break;
                }
        }

        private void RunToStop() => botStats.IsRunning = false;
        private void RotateToRun() => botStats.IsRotating = false;
        private void TurnOfEdgeFall() => botStats.IsFallingEdge = false;
      
    }
}
