using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//May need a namespace here in the future but for now it seems to work fine

public static class DebugEx
{
    /// <summary>
    ///	Easier way to log and change types
    /// </summary>
    public static void LogWithType(string _str, LogType _logType)
    {
        switch (_logType)
        {
            case LogType.Log:
                Debug.Log(_str);
                break;
            case LogType.Warning:
                Debug.LogWarning(_str);
                break;
            case LogType.Error:
                Debug.LogError(_str);
                break;
            default:
                Debug.Log($"[Unhandled LogType] {_str}");
                break;
        }
    }
    public static void CondLog(bool _cond, string _str1, string _str2, LogType _logType1 = LogType.Log, LogType _logType2 = LogType.Error)
    {
        if(_cond)
        {
            LogWithType(_str1, _logType1);
        }
        else
        {
            LogWithType(_str2, _logType2);
        }
    }
    public static void LogNull<T>(this T obj, string _objName, LogType _ifNullType = LogType.Error, LogType _ifNotType = LogType.Log)
    {
        if (obj == null)
        {
            LogWithType(String.Format("{0} IS null.", _objName), _ifNullType);
        }
        else
        {
            LogWithType(String.Format("{0} is NOT null.", _objName), _ifNotType);
        }
    }

}
