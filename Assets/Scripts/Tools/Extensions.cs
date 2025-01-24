using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro.EditorUtilities;


//May need a namespace here in the future but for now it seems to work fine

public static class Extensions
{
    public static List<AbilityEff> cloneEffects(this List<AbilityEff> AE_list){
        List<AbilityEff> listToReturn = new List<AbilityEff>();
        if(AE_list.Count > 0){
            for(int i=0; i < AE_list.Count; i++){
                listToReturn.Add(AE_list[i].clone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    
    public static void addEffect(this List<EffectInstruction> _eInstruct_list, AbilityEff _effect){
        EffectInstruction tempRef = new EffectInstruction(_effect, 0);
        _eInstruct_list.Add(tempRef);
    }
    public static void addEffect(this List<EffectInstruction> _eInstruct_list, AbilityEff _effect, int _targetArg){
        EffectInstruction tempRef = new EffectInstruction(_effect, _targetArg);
        _eInstruct_list.Add(tempRef);
    }
    public static void addEffects(this List<EffectInstruction> _eInstruct_list, List<AbilityEff> _effects){
        foreach (AbilityEff eff in _effects){
            _eInstruct_list.addEffect(eff);
        }
    }
    public static void addEffects(this List<EffectInstruction> _eInstruct_list, List<AbilityEff> _effects, List<int> _targetArgs){
        if(_effects.Count == _targetArgs.Count){
            Debug.LogWarning("addEffects called with less tartgetArgs than effects. Remainder will be set to 0");
        }
        for (int i = 0; i < _effects.Count; i++)
        {
            if(i <_targetArgs.Count){
                _eInstruct_list.addEffect(_effects[i], _targetArgs[i]);
            }
            else{
                _eInstruct_list.addEffect(_effects[i], 0);
            }
        }
    }
    public static List<EffectInstruction> cloneInstructs(this List<EffectInstruction> eI_list){
        List<EffectInstruction> listToReturn = new List<EffectInstruction>();
        if(eI_list.Count > 0){
            for(int i=0; i < eI_list.Count; i++){
                listToReturn.Add(eI_list[i].clone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static List<EffectInstruction> cloneInstructsNoEffectClone(this List<EffectInstruction> eI_list){
        List<EffectInstruction> listToReturn = new List<EffectInstruction>();
        if(eI_list.Count > 0){
            for(int i=0; i < eI_list.Count; i++){
                listToReturn.Add(eI_list[i].cloneNoEffectClone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static NetworkConnection GetNetworkConnection(this MonoBehaviour _mo){
        return _mo.GetComponent<NetworkIdentity>().connectionToClient;
    }
    public static Vector2 BottomCenter(this Bounds _bounds)
    {
        Vector2 toReturn = new Vector2();

        toReturn.x = _bounds.center.x;
        toReturn.y = _bounds.min.y;
        
        return toReturn;
    }

    public static Vector2 TopCenter(this Bounds bounds)
    {
        return new Vector2()
        {
            x = bounds.center.x,
            y = bounds.max.y
        };
    }

    public static bool RemoveRandom<T>(this IList<T> _listToCheck, Predicate<T> _matchExpression)
    {

        List<int> indices = new List<int>();

        // Finding indices that match the predicate
        for(int i = 0; i < _listToCheck.Count; i++)
        {
            if( _matchExpression(_listToCheck[i]) )
            {
                indices.Add(i);
            }
        }

        // If none found return false
        if(indices.Count <= 0)
        {
            return false;
        }

        // Removing random object that matches
        int randomIndex = indices[UnityEngine.Random.Range(0, indices.Count)];

        _listToCheck.RemoveAt(randomIndex);

        return true;
    
    }
    /// <summary>
    ///	Returns List<int> of indices that match the predicate. null if none found
    /// </summary>
    public static List<int> IndicesThatMatch<T>(this IList<T> _listToCheck, Predicate<T> _matchExpression)
    {

        List<int> indices = new List<int>();

        // Finding indices that match the predicate
        for(int i = 0; i < _listToCheck.Count; i++)
        {
            if( _matchExpression(_listToCheck[i]) )
            {
                indices.Add(i);
            }
        }

        // If none found return null
        if(indices.Count <= 0)
        {
            return null;
        }

        return indices;
    
    }
    /// <summary>
    ///	Safe way to get transform of Monobeviour. Returns null is the MO is null
    /// </summary>
    public static Transform transformSafe(this MonoBehaviour _mo)
    {
        try
        {
            return _mo.transform;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    ///	Safe way to get transform of GameObject. Returns null is the GO is null
    /// </summary>
    public static Transform transformSafe(this GameObject _go)
    {
        try
        {
            return _go.transform;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    ///	Safe way to a Comp from a transform. Will return null if transform is null
    /// </summary>
    public static T GetComponentSafe<T>(this Transform _trans)
    {
        try
        {
            return _trans.GetComponent<T>();
        }
        catch
        {
            return default;
        }
    }
    /// <summary>
    ///	TryGetCompontent but safe. Will return false if called on null transform. This may be slow be slow. I'd be careful using it too much
    /// </summary>
    public static bool TryGetComponentSafe<T>(this Transform _trans, out T _someComp)
    {
        try
        {
            if(_trans.TryGetComponent(out T _outC))
            {
                _someComp = _outC;
                return true;
            }
        }
        catch{}

        _someComp = default(T);
        return false;
    }
    /// <summary>
    ///	Convient way to scale vectors. Output is new vector
    /// </summary>
    public static Vector2 QuickScale(this Vector2 _vect, float _x, float _y)
    {
        return new Vector2(_vect.x * _x, _vect.y * _y);
    }
    /// <summary>
    ///	Convient way to scale vectors. Output is new vector
    /// </summary>
    public static Vector3 QuickScale(this Vector3 _vect, float _x, float _y, float _z)
    {
        return new Vector3(_vect.x * _x, _vect.y * _y, _vect.z * _z);
    }

}
