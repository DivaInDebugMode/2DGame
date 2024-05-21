using System;
using UnityEngine;
using DG.Tweening;
using Platform.Interface;

namespace Platform
{
    public class MovingPlatform : MonoBehaviour, IMovablePlatform
    {
        [SerializeField] private Transform platform;
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;
        [SerializeField] private float movingSpeed;
        [SerializeField] private float stopThreshold = 0.01f;
        private int direction = 1;
        private bool isMoving = true; 

        private void Start()
        {
            Move();
        }

        private Vector2 CurrentMovementTarget()
        {
            return direction == 1 ? endPos.position : startPos.position;
        }

        private void ChangeDirectionIfNeeded(Vector2 target, Vector2 position)
        {
            if (!((target - position).sqrMagnitude < stopThreshold * stopThreshold)) return;
            direction *= -1;
            isMoving = true; // Reset isMoving flag when changing direction
        }
        
        public void Move()
        {
            if (!isMoving)
                return; // Stop moving if the platform is not supposed to move

            var target = CurrentMovementTarget();
            // Calculate duration based on distance and speed
            var distance = Vector2.Distance(platform.position, target);
            var duration = distance / movingSpeed;

            // Tween the platform's position
            platform.DOMove(target, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                ChangeDirectionIfNeeded(target, target);
                Move(); // Move again after reaching the target
            });
        }
        
        
    }
}