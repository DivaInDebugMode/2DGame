using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private List<Material> materials;
    [SerializeField] private float targetValue;
    [SerializeField] private float startValue; 
    [SerializeField] private float duration;
    private static readonly int Power = Shader.PropertyToID("_Power");
    

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

    public void DissolvePLayer() => StartCoroutine(ChangeDissolveStrength(targetValue));
    public void AppearPlayer() => StartCoroutine(ChangeDissolveStrength(startValue));
   
}
