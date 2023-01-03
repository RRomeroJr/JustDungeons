using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : BuffEffect
{
    public override void EndEffect(IBuff t, float s)
    {
        var target = t as ISpeedModifier;
        if (target != null)
        {
            target.SpeedModifier = 1 / s;
        }
    }

    public override void StartEffect(IBuff t, float s)
    {
        var target = t as ISpeedModifier;
        if (target != null)
        {
            target.SpeedModifier = s;
        }
    }
}
