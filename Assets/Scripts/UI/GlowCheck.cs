using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName="GlowCheck", menuName = "HBCsystem/UI/GlowCheck")]
public class GlowCheck: ScriptableObject
{
	public Ability_V2 ability;
    public bool active = false;
    [SerializeField]
    
    public UnityEvent glowChecks;
    private UnityEvent ActivateCheck;
    private UnityEvent DeactivateCheck;

    public void CheckConditions()
    {
        glowChecks.Invoke();
    }
    public void GlowSelfCheck(){
        Debug.Log("GlowSelfCheck.." + name);
        if(ability == null){
            return;
        }
        bool _cond = UIManager.playerActor.getHealthPercent() < 0.9f;
        if(_cond && !active)
        {
            Debug.Log(ability.name + ".. Start Glowing!");
            UIManager.Instance.glowList.Add(ability);
            active = true;
        }
        else if(!_cond && active)
        {
            Debug.Log(ability.name + ".. Stop Glowing!");
            UIManager.Instance.glowList.Remove(ability);
            active = false;
        }
        // if(_cond && !active)
        // {
        //     Debug.Log(ability.name + ".. Start Glowing!");
        //     UIManager.Instance.glowList.Add(ability);
        //     active = true;
        // }
        // else if(!_cond && active)
        // {
        //     Debug.Log(ability.name + ".. Stop Glowing!");
        //     UIManager.Instance.glowList.Remove(ability);
        //     active = false;
        // }
        // if(!active){
        //     Debug.Log(ability.name + ".. Start Glowing!");
        //     UIManager.Instance.glowList.Add(ability);
        //     active = true;
        // }
        // else{
        //     Debug.Log(ability.name + ".. Start Glowing!");
        //     UIManager.Instance.glowList.Remove(ability);
        //     active = false;
        // }
       
    }
    public void FillerGlowCheck(){
        //Debug.Log("FillerGlowCheck.." + name);
        if(ability == null){
            return;
        }
        bool _cond = false;
        try
        {
            _cond = HBCTools.checkIfBehind(UIManager.playerActor, UIManager.playerActor.target);
        }
        catch
        {
           
        }
        if(_cond && !active)
        {
            // Debug.Log(ability.name + ".. Start Glowing!");
            UIManager.Instance.glowList.Add(ability);
            active = true;
        }
        else if(!_cond && active)
        {
            // Debug.Log(ability.name + ".. Stop Glowing!");
            UIManager.Instance.glowList.Remove(ability);
            active = false;
        }
    }
    public void GlowSelfActivateEx(){
        Ability_V2 _smallGen1Ref = AbilityData.instance.findByName("SmallGen1");
        if(_smallGen1Ref == null){
            return;
        }
        Debug.Log(_smallGen1Ref.name + ".. Start Glowing!");
        UIManager.Instance.glowList.Add(_smallGen1Ref);
        active = true;
    }
    public void GlowSelfDeactivateEx(){
        Ability_V2 _smallGen1Ref = AbilityData.instance.findByName("SmallGen1");
        if(_smallGen1Ref == null){
            return;
        }
        Debug.Log(_smallGen1Ref.name + ".. Start Glowing!");
        UIManager.Instance.glowList.Remove(_smallGen1Ref);
        active = false;
    }


}