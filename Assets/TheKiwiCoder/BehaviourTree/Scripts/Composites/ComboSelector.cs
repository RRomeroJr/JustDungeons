using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class ComboSelector : CompositeNode {
        protected int current;

        protected override void OnStart() {
            Multiboss _comp = context.gameObject.GetComponent<Multiboss>();
            if(_comp != null){
                current = (int)_comp.lastCombo;
            }
            else{
                current = -1;
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if(current < 0 || children.Count <= current){
                return State.Failure;
            }
            var child = children[current];

            switch (child.Update()) {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    return State.Failure;
            }
            
            Debug.LogError(GetType().ToString() +"| Got out of switch without returning");
            return State.Failure;
        }
    }
}