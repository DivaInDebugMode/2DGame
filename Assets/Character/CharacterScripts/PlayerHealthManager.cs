using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class PlayerHealthManager : MonoBehaviour
    {
        private readonly UnitHealth playerHealth = new(0,0);
        public event Action OnPlayerDeath;
        
        public void PlayerTakeDamage(int damageAmount)
        {
            playerHealth.DamageUnit(damageAmount);
            OnPlayerDeath?.Invoke();
        }

        private void PlayerHeal()
        {
            playerHealth.HealUnit(0);
        }
    }
}