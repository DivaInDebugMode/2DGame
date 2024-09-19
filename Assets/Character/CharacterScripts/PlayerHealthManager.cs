using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class PlayerHealthManager : MonoBehaviour
    {
        private readonly UnitHealth playerHealth = new(0,0); 
        [SerializeField] private Dissolve dissolve;
        [SerializeField] private BotData botData;
        [SerializeField] private PlayerInput botInput;
        public BotData Data => botData;
        public PlayerInput BotInput => botInput;

        public Dissolve PlayerDissolve => dissolve;
        public event Action OnPlayerRespawn;

        public void PlayerTakeDamage(int damageAmount)
        {
            playerHealth.DamageUnit(damageAmount); 
            dissolve.DissolvePLayer();
            botInput.enabled = false;
            OnPlayerRespawn?.Invoke();
           
        }

        private void PlayerHeal()
        {
            playerHealth.HealUnit(0);
        }
    }
}