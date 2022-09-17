using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NullibleVector3
{
    // Nullible Vector3 Wrapper class
    // Unity can't serialize things like Vector3? so this is a workaround
    // Wrap a Value type in this class and now you can just check if the 
    // class reference is null. It isn't the value is NullibleWrapper<T>.value
    public Vector3 Value;
    
}
