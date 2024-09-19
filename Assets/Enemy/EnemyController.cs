using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb; 
        [SerializeField] private Animator animator; 
        [SerializeField] private List<Transform> patrolPoints; 
        [SerializeField] private float patrolSpeed;
        [SerializeField] private float chaseSpeed; 
        [SerializeField] private float firstPointStopDuration;
        [SerializeField] private float secondPointStopDuration;
        [SerializeField] private int patrolIndex;
        [SerializeField] private float detectionRange; 
        [SerializeField] private float attackRange; 
        [SerializeField] private float groundCheckDistance; 
        [SerializeField] private Transform groundCheckTransform;
        [SerializeField] private LayerMask playerLayer; 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private bool isBackwards;
        [SerializeField] private int backwardsRadius;
        private float distanceFromPoint;
        private float targetAngle; 
        private bool isChasing; 
        private bool isGrounded; 
        private bool isAttacking; 
        private bool isStopping;
        private Transform playerTransform; // Reference to the player's transform
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        private void Start()
        {
            isBackwards = false;
        }

        private void Update()
        {
            isGrounded = CheckIfGrounded();
            CheckPlayerBackwards();
            if (!isGrounded && isChasing && !isStopping)
            {
                isChasing = false;
                isStopping = true;
                animator.SetBool(IsIdle, true);
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsRunning, false);
                animator.SetBool(IsAttacking, false);
                rb.velocity = Vector2.zero;
                StartCoroutine(StopForSeconds(2f)); 
            }

            if (isGrounded && !isAttacking && !isStopping)
            {
                Vector3 direction = transform.forward;

                if (Physics.Raycast(transform.position, direction, out var hit, detectionRange, playerLayer))
                {
                    if (hit.collider is not null)
                    {
                        playerTransform = hit.transform;
                        isChasing = true;
                    }
                }
                else
                {
                    isChasing = false;
                    playerTransform = null;
                }
            }

            if (isChasing && playerTransform is not null)
            {
                var distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer <= attackRange)
                {
                    isChasing = false;
                    isAttacking = true;
                    StartCoroutine(AttackPlayer());
                }
            }

            if (!isChasing && !isAttacking && !isStopping)
            {
                distanceFromPoint = Mathf.Abs(transform.position.x - patrolPoints[patrolIndex].transform.position.x);

                if (distanceFromPoint <= 0.1f)
                {
                    switch (patrolIndex)
                    {
                        case 0:
                            StartCoroutine(StopAtFirstPoint());
                            break;
                        case 1:
                            StartCoroutine(StopAtSecondPoint());
                            break;
                    }
                }
            }

            SmoothRotate();

            if (playerTransform is null && isBackwards)
            {
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsIdle, true);
                animator.SetBool(IsRunning, false);
                animator.SetBool(IsAttacking, false);
            }
        }

        private void FixedUpdate()
        {
            if (isChasing && isGrounded && !isStopping)
            {
                ChasePlayer();
            }
            else if (!isAttacking && !isStopping)
            {
                Patrolling();
            }
        }

        private bool CheckIfGrounded()
        {
            return Physics.Raycast(groundCheckTransform.position, Vector3.down,
                out _, groundCheckDistance, groundLayer);
        }

        private void Patrolling()
        {
            if(isBackwards) return;
            var targetPoint = patrolPoints[patrolIndex];
            Vector2 direction = (targetPoint.position - transform.position).normalized;

            if (direction.x > 0)
                targetAngle = 90f;
            else
                targetAngle = 270f;

            rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);

            animator.SetBool(IsWalking, true);
            animator.SetBool(IsIdle, false);
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsAttacking, false);
        }

        private void ChasePlayer()
        {
            if (playerTransform is not null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                targetAngle = direction.x > 0 ? 90f : 270f; 

                rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
                
                animator.SetBool(IsRunning, true);
                animator.SetBool(IsWalking, false);
                animator.SetBool(IsIdle, false);
                animator.SetBool(IsAttacking, false);
                Debug.Log("chaseee");
            }
        }

        private void CheckPlayerBackwards()
        {
            isBackwards = Physics.CheckSphere(new Vector2(transform.position.x , transform.position.y + 4f), backwardsRadius, playerLayer);
            Collider[] colliders = Physics.OverlapSphere(transform.position, backwardsRadius, playerLayer);

            if (colliders.Length > 0 && isBackwards)
            {
                rb.velocity = Vector3.zero;
                var targetPoint = colliders[0].transform;
                Vector2 direction = (targetPoint.position - transform.position).normalized;

                targetAngle = direction.x > 0 ? 90f : 270f;
                
            }
        }
        
        private void OnDrawGizmos()
        {
            Vector3 checkPosition = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
            Gizmos.color = Color.red; // Color of the gizmo
            Gizmos.DrawWireSphere(checkPosition, backwardsRadius); // Draw the wireframe sphere
        }

        private IEnumerator AttackPlayer()
        {
            rb.velocity = Vector2.zero;

            animator.SetBool(IsAttacking, true);
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsIdle, false);

            yield return new WaitForSeconds(1f); 

            isAttacking = false;
        }

        private IEnumerator StopForSeconds(float duration)
        {
            yield return new WaitForSeconds(duration);
            isStopping = false;
        }

        private void SmoothRotate()
        {
            var currentAngle = transform.eulerAngles.y;

            if (Mathf.Abs(currentAngle - targetAngle) > 0.0001f)
            {
                rb.velocity = Vector2.zero;

                var targetRotation = Quaternion.Euler(0, targetAngle, 0); 
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        private IEnumerator StopAtFirstPoint()
        {
            rb.velocity = Vector2.zero;

            animator.SetBool(IsIdle, true);
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsAttacking, false);

            yield return new WaitForSeconds(firstPointStopDuration);
            patrolIndex = 1; 
        }

        private IEnumerator StopAtSecondPoint()
        {
            rb.velocity = Vector2.zero;

            animator.SetBool(IsIdle, true);
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsAttacking, false);

            yield return new WaitForSeconds(secondPointStopDuration);
            patrolIndex = 0;
        }
    }
}