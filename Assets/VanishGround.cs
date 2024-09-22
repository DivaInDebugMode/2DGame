using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishGround : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> grounds;
    [SerializeField] private List<Material> materials;
    [SerializeField] private float targetValue;
    [SerializeField] private float duration;
    [SerializeField] private float disappearTime;
    private static readonly int Power = Shader.PropertyToID("_GroundPower");
    [SerializeField] private LayerMask playerLayer;
    private void Start()
    {
        foreach (var material in materials)
        {
            material.SetFloat(Power,0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            DisableGround();
        }
    }

    private void DisableGround()
    {
        StartCoroutine(DisableGroundTimer());
    }

    private IEnumerator DisableGroundTimer()
    {
        StartCoroutine(ChangeDissolveStrength(targetValue));
        yield return new WaitForSecondsRealtime(disappearTime);
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