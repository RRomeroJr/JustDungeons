using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/* Richie:
    The idea here is that objects in the world can 
    look antoher object with this script on it to 
    activate/ deactivate.

    I kept it seperate instead of inhertiting this class
    to create other trigger subclasses bc I wansn't sure
    how/ if you can do some sort of inheritance thing with
    GetComponent()<>

    For now the working idea I had is that any trigger 
    component we make will set this classes's isActive to
    true/ false appropriatly. Then all other objects have 
    to do to is see if isActive is true to do stuff.

    Ex. AreaTrigger checks if player is in a object ->

        if true set isActive true in TriggerBase ->

        A door with a reference to that object sees
        it's TriggerBase's isActive is true and opens
*/
public class TriggerBase : MonoBehaviour
{
    public bool isActive = false;
}
