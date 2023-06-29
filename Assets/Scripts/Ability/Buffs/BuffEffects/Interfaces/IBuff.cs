using BuffSystem;
using System;
using System.Collections.Generic;
/// <summary>
/// Allows objects to accepts buffs. Every buff interface inherits IBuff
/// </summary>
public interface IBuff
{
    public void AddBuff(BuffScriptableObject buff);

    public void RemoveBuff(Buff buff);
    public bool RemoveRandomBuff(Predicate<BuffSystem.Buff> _matchExpression);
}
