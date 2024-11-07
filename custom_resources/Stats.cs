using Godot;

namespace DeckBuilder;

[GlobalClass]
public partial class Stats : Resource
{

    [Signal] public delegate void StatsChangedEventHandler();

    [Export] public int maxHealth = 1;
    [Export] public Texture2D art;

    public int health {
        get => _health;
        set => SetHealth(value);
    }
    private int _health;
    public int block {
        get => _block;
        set => SetBlock(value);
    }
    private int _block;

    public void SetHealth(int value)
    {
        _health = Mathf.Clamp(value, 0, maxHealth);
        EmitSignal(Stats.SignalName.StatsChanged);
    }

    public void SetBlock(int value)
    {
        _block = Mathf.Clamp(value, 0, 999); // 999 is the max block from Slay the Spire
        EmitSignal(Stats.SignalName.StatsChanged);
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage < 0) return;

        int initialDamage = damage;
        damage = Mathf.Clamp(damage - block, 0, damage);
        block -= Mathf.Clamp(block - initialDamage, 0, block);
        health -= damage;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public Stats CreateInstance()
    {
        Stats instance = Duplicate() as Stats;
        instance.health = maxHealth;
        instance.block = 0;
        return instance;
    }

}
