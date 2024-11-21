using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;

    private int health;
    private int maxHealth;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return (float)health / maxHealth;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);  // Trigger health change event

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);  // Trigger health change event
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);  // Trigger death event
    }
}
