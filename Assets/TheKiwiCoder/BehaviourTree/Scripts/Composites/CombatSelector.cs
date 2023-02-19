using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class CombatSelector : CompositeNode {
        

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            
                Node childToRun;
                if(context.actor.inCombat == false){
                    childToRun = children[0];
                }
                else{
                    childToRun = children[1];
                }

                switch (childToRun.Update()) {
                    case State.Running:
                        return State.Running;
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        break;
                }
            
            

            return State.Failure;
        }
    }
}