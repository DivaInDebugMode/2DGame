using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class PlayerHealthManager : MonoBehaviour
    {
        private readonly UnitHealth playerHealth = new(0,0); 
        [SerializeField] private Dissolve dissolve;
        [SerializeField] private BotData botData;

        public BotData Data => botData;

        public Dissolve PlayerDissolve => dissolve;
        public event Action OnPlayerRespawn;

        public void PlayerTakeDamage(int damageAmount)
        {
            playerHealth.DamageUnit(damageAmount); 
            dissolve.DissolvePLayer();
            OnPlayerRespawn?.Invoke();
           
        }

        private void PlayerHeal()
        {
            playerHealth.HealUnit(0);
        }
    }
}