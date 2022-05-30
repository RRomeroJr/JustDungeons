using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    
    public UIManager uiManager;
    public GameObject player;

    // ~~~~~~~~~~~~~~~~~~~~~~For testing casting and spell effect system~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // RR: In the future these should be in a sperate file somewhere. Don't know how to do that yet
    public SpellEffect oneOffDamageEffect;
    public SpellEffect dotEffect;
    public SpellEffect oneOffHealEffect;
    public SpellEffect hotEffect;
    public Spell castedSpell;
    public Spell instantSpell;
    public Spell castedHeal;
    public Spell instantHeal;
    public Spell testerBolt;
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    void Start()
    {
        Debug.Log("Press \"1-4\" | DoT, Dmg, Heal, HoT! Careful bc you can do many at once if you spam");
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~For testing casting and spell effect system~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // RR: In the future these should be in a sperate file somewhere. Don't know how to do that yet
        
        //            (Spell Name, Spell Type, Power, Duration, Tick Rate) || 0=dmg, 1=heal, tick rate not working atm
        oneOffDamageEffect = new SpellEffect("Testerbolt Effect", 0, 7.0f, 0.0f, 0.0f);
        dotEffect = new SpellEffect("Debugger\'s Futility Effect", 2, 30.0f, 9.0f, 3.0f);// damage ^^
        oneOffHealEffect = new SpellEffect("Quality Assured Effect", 1, 13.0f, 0.0f, 0.0f);
        hotEffect = new SpellEffect("Sisyphean Resolve Effect", 3, 25.0f, 4.0f, 1.0f);// heals ^^

        castedSpell = new Spell("Testerbolt", oneOffDamageEffect, 1.5f);
        instantSpell = new Spell("Debugger\'s Futility", dotEffect, 0.0f);
        castedHeal = new Spell("Quality Assured", oneOffHealEffect, 1.5f);
        instantHeal = new Spell("Sisyphean Resolve Effect", hotEffect, 0.0f);
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }

    void Update()
    {
        
        if(Input.GetKeyDown("1")){
                castSpell(instantSpell);
        }
        if(Input.GetKeyDown("2")){
                
                castSpell(castedSpell);
        }
        if(Input.GetKeyDown("3")){
                castSpell(castedHeal);
        }
        if(Input.GetKeyDown("4")){
                
                castSpell(instantHeal);
        }
    }
    void castSpell(Spell inSpell){
        if(uiManager.hasTarget){
            //Debug.Log("GM: Casting Spell.. " + inSpell.getName());
            if(inSpell.getCastTime() > 0.0f){
                //Creating cast bar and setting it's parent to canvas to display it properly
                GameObject newSpellCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                // v (SpellEffect spell_effect, string spell_name, Actor from_caster, Actor to_target, float cast_time) v
                newSpellCast.GetComponent<SpellCast>().Init(inSpell.getEffect(), inSpell.getName(), player.GetComponent<Actor>(),
                                                                uiManager.targetFrame.actor, inSpell.getCastTime());
            }
            else{
                Debug.Log("GM| Instant cast: " + inSpell.getName());
                uiManager.targetFrame.actor.applySpellEffect(inSpell.getEffect(), player.GetComponent<Actor>());
            }
        }
        else{
            Debug.Log("You don't have a target!");
        }
        
    }
}
