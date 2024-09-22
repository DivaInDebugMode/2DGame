using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room1PlayerTriggerManager : MonoBehaviour
{
    [SerializeField] private MovingWalls movingWalls;
    [SerializeField] private List<GameObject> grounds;
    [SerializeField] private List<Material> materials;
    [SerializeField] private float targetValue;
    [SerializeField] private float duration;
    private static readonly int Power = Shader.PropertyToID("_GroundPower");
    
    private void OnEnable()
    {
        movingWalls.OnWallStopped += DisableGround;
    }

    private void OnDisable()
    {
        movingWalls.OnWallStopped -= DisableGround;
    }

    private void Start()
    {
        foreach (var material in materials)
        {
            material.SetFloat(Power,0);
        }
    }

    private void DisableGround()
    {
        StartCoroutine(DisableGroundTimer());
    }

    private IEnumerator DisableGroundTimer()
    {
        StartCoroutine(ChangeDissolveStrength(targetValue));
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var ground in grounds)
        {
            ground.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator ChangeDissolveStrength(float endValue)
    {
        foreach (var material in materials)
        {
            var currentValue = material.GetFloat(Power);
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                var newValue = Mathf.Lerp(currentValue, endValue, elapsedTime / duration);
                material.SetFloat(Power, newValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            material.SetFloat(Power, endValue);
        }
    }
}
