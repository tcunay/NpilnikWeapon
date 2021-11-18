class Weapon
{
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
    private readonly Health _health = new Health();

    public bool IsDead => _health.IsDead;

    public void TakeDamage(int damage)
    {
        if (IsDead == false)
            _health.TakeDamage(damage);
    }
}

public interface IDamageable
{
    bool IsDead { get; }
    void TakeDamage(int damage);
}

class Health : IDamageable
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
}