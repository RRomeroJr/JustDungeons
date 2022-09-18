using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyController : MonoBehaviour
{
    public EnemySO enemyStats;
    // The main behaviour tree asset
    public BehaviourTree tree;
    public List<Ability_V2> abilities;

    private Actor actor;

    // Storage container object to hold game object subsystems
    Context context;
    public Vector3 spawnLocation;
    private Dictionary<string, object> extra = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.position;
        ExtraValues();
        context = Context.CreateFromGameObject(gameObject, extra, enemyStats);
        tree = tree.Clone();
        tree.Bind(context);
        spawnLocation = transform.position;
        actor = gameObject.GetComponent<Actor>();
    }

    // Any extra values you want the behavior tree to have access to should be added here
    void ExtraValues()
    {
        extra["spawnLocation"] = spawnLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if (tree)
        {
            tree.Update();
        }
        /*for(i = 0; i < AbilityList.Count(); i++){
            if(AbilityList[i].CD is ready?){
                
                // Resolve AbilityList[i] some how
                // maybe AbilityList has a some field that corresponds
                // to how the Ability should be cast.
                // 0 = onClostest, 1 = onHightestThreat, ect.
                
            }
        }
        */
    }
    // RR: Now that I'm writing this out it would be easier if 
    //     abilitys had cooldowns before I start doing this

    //void onClosest(Ability)
    //void onHighestThreat(Ability)
    //void onLowestTreat()
    //void atPercentHealth(Ability)
    //void onHealer()
    //void onRanged()
    //void onMelee()
    //void onTank()
}
