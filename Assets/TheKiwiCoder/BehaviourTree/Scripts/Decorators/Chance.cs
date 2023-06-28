using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    /// <summary>
    /// Node that has a chance to update it's child,
    /// </summary>
    public class Chance : DecoratorNode {
        public float chanceToProceed = 1.0f;
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            float roll = (float)Random.Range(0, 101) / 100.0f;
            if(roll == 0.0f)
            {
                Debug.Log("Roll was 0");
                return State.Failure;
            }
            
            if(roll > chanceToProceed)
            {
                Debug.Log(roll + " > " + chanceToProceed);
                return State.Failure;
            }
            
            return child.Update();
        }
    }
}