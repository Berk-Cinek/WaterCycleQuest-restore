public interface IDamageable
{
    void Damage(int damageAmount);

    event System.Action OnDeath;

}
