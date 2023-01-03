using System.Collections;
using System.Collections.Generic;
using BuffSystem;
using UnityEngine;

public interface ISpeedModifier : IBuff
{
    public float SpeedModifier { get; set; }
}
