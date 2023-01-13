using BuffSystem;
/// <summary>
/// Allows objects to accepts buffs. Every buff interface inherits IBuff
/// </summary>
public interface IBuff
{
    public void AddBuff(BuffScriptableObject buff);

    public void RemoveBuff(Buff buff);
}
