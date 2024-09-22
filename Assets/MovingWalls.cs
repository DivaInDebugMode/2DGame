using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovingWalls : MonoBehaviour
{
   [SerializeField] private LayerMask playerLayer;
   [SerializeField] private bool hasEntered;
   [SerializeField] private bool hasOpened;
   [SerializeField] private Transform liftWall;
   [SerializeField] private float liftSpeed;
   [SerializeField] private float liftYAmount;
   public event Action OnWallStopped;
   private Vector3 targetPosition;
   
   private void Start()
   {
      targetPosition = liftWall.position;
      hasOpened = false;
   }

   private void FixedUpdate()
   {
      if (hasEntered)
      {
         liftWall.position = Vector3.MoveTowards(liftWall.position, targetPosition, liftSpeed * Time.fixedDeltaTime);
            
         if (Vector3.Distance(liftWall.position, targetPosition) < 0.01f)
         {
            OnWallStopped?.Invoke();
            hasEntered = false;
         }
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if(hasOpened) return;
      if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
      {
         hasEntered = true;
         
         targetPosition = new Vector3(liftWall.position.x, liftWall.position.y + liftYAmount, liftWall.position.z);
         hasOpened = true;
      }
   }
}