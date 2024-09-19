using System.Collections.Generic;
using Character.CharacterScripts;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private List<CollisionDetection> collisionDetection = new();

    private void OnEnable()
    {
        foreach (var collision in collisionDetection)
        {
            collision.OnPlayerDetection += playerHealthManager.PlayerTakeDamage;
        }
    }

    private void OnDisable()
    {
        foreach (var collision in collisionDetection)
        {
            collision.OnPlayerDetection -= playerHealthManager.PlayerTakeDamage;
        }
    }
}