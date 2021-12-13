class Weapon
{
    public event Action BulletsRunOut;
    public int Damage { get; private set; }
    public int Bullets { get; private set; }
    public bool CanFire => Bullets > 0;

    public void Fire(IDamageable damageable)
    {
        if (CanFire)
        {
            Bullets--;
            damageable.TakeDamage(Damage);
        }
        else
        {
            BulletsRunOut?.Invoke();
        }
    }
}

public class Bot
{
    private readonly Weapon _weapon = new Weapon();

    public void OnSeePlayer(IDamageable player)
    {
        _weapon.Fire(player);
    }
}

public class Player : IDamageable
{
    public event Action Dead;
    public int Value { get; private set; }
    public bool IsDead => Value <= 0;

    public void TakeDamage(int damage)
    {
        if (damage < 0 || IsDead)
            throw new InvalidOperationException();

        if (damage >= Value)
        {
            Value = 0;
            Dead?.Invoke();
            return;
        }

        Value -= damage;

        if (Value < 0)
            throw new InvalidOperationException();
}

public interface IDamageable
{
    bool IsDead { get; }
    void TakeDamage(int damage);
}

class Health : IDamageable
{
    
    }
}