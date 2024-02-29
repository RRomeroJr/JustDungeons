using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public void DestroyWindow()
    {
        Destroy(gameObject);
    }
}