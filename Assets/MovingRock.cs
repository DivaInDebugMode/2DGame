using System.Collections;
using UnityEngine;

public class MovingRock : MonoBehaviour
{
    [SerializeField] private float startMovingTime;
    [SerializeField] private float movingSpeed;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private bool isStopped;
    [SerializeField] private float stopDuration;
    private float movingTime;
    private int direction;
  
    private void Start()
    {
        direction = 1;
        movingTime = startMovingTime;
        isStopped = false;
    }

    private void FixedUpdate()
    {
        if (isStopped) return;
        if (movingTime <= 0)
        {
            direction = -direction;
            movingTime = startMovingTime;
            StartCoroutine(StopPlatform());
        }
        else
        {
            movingTime -= Time.deltaTime;
        }
        
        gameObject.transform.Translate(moveDirection * (movingSpeed * direction * Time.deltaTime));
      
    }
    private IEnumerator StopPlatform()
    {
        isStopped = true;
        yield return new WaitForSeconds(stopDuration);
        isStopped = false;
    }
}