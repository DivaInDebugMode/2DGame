namespace Character.CharacterScripts
{
    public class UnitHealth
    {
        private int currentHealth;
        private int currentMaxHealth;

        public int Health
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public int MaxHealth
        {
            get => currentMaxHealth;
            set => currentMaxHealth = value;
        }

        public UnitHealth(int health, int maxHealth)
        {
            currentHealth = health;
            currentMaxHealth = maxHealth;
        }

        public void DamageUnit(int damageAmount)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damageAmount;
            }
        }
    
        public void HealUnit(int healAmount)
        {
            if (currentHealth < currentMaxHealth)
            {
                currentHealth += healAmount;
            }

            if (currentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            }
        }
    }
}