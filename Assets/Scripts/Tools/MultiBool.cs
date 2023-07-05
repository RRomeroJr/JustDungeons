using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MultiBool
{
    public uint value;


    public MultiBool(bool _in){

        if(_in)
        {
            value = 1;
        }
        else
        {
            value = 0;
        }

    }
    public MultiBool(int _in){

        value = (uint)_in;

    }
    public MultiBool(uint _in){

        value = _in;

    }
    public static bool operator true(MultiBool _in)
    {
        return _in.value > 0;
    }
    public static bool operator false(MultiBool _in)
    {
        return _in.value == 0;
    }
    public static implicit operator bool(MultiBool _in)
    {
        return _in.value > 0;
    }
    public static implicit operator int(MultiBool _in)
    {
        return (int)_in.value;
    }
    public static implicit operator uint(MultiBool _in)
    {
        return _in.value;
    }
    public static MultiBool operator ++(MultiBool _in)
    {
        _in.value++;
        return _in;
    }
    public static MultiBool operator --(MultiBool _in)
    {
        _in.value--;
        return _in;
    }
    public static implicit operator MultiBool(int _in)
    {
        return new MultiBool(_in);
    }
    public static bool operator ==(MultiBool left, bool right)
    {
        return left == right;
    }
    public static bool operator !=(MultiBool left, bool right)
    {
        return left != right;
    }
    public static bool operator ==(bool left, MultiBool right)
    {
        return right == left;
    }
    public static bool operator !=(bool left, MultiBool right)
    {
        return right == left;
    }
    public static int operator +(int left, MultiBool right)
    {
        right.value += (uint)left;
        return right;
    }
    public static int operator +(MultiBool left, int right)
    {
        left.value += (uint)right;
        return left;
    }
    public static int operator -(int left, MultiBool right)
    {
        right.value -= (uint)left;
        return right;
    }
    public static int operator -(MultiBool left, int right)
    {
        left.value -= (uint)right;
        return left;
    }
    void test()
    {
        MultiBool mb = new MultiBool(true);

        bool comp = true;
        int thing = 2;

        if(comp == mb)
        {
            Debug.Log("all good");
        }
        if(comp)
        {
            mb++;
            mb += 2;
        }
        else
        {
            mb--;
        }
        
        if(!mb)
        {
            mb = 4;
            Debug.Log("stuff");
        }
    }
    


}