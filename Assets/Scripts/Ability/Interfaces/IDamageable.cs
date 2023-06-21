public interface IDamageable
{
    public int Health { get; set; }
    void Damage(float amount);
}
