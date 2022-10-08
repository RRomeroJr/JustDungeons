using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class RepeatXTimes : DecoratorNode{

        public bool restartOnSuccess = true;
        public bool restartOnFailure = false;
        public int count = 1;
        int counter;
        
        protected override void OnStart() {
            counter = count;
        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {
            if(counter > 0){
                switch (child.Update()) {
                    case State.Running:
                        break;
                    case State.Failure:
                        if (restartOnFailure) {
                            counter -= 1;
                            return State.Running;
                        } else {
                            return State.Failure;
                        }
                    case State.Success:
                        if (restartOnSuccess) {
                            counter -= 1;
                            return State.Running;
                        } else {
                            return State.Success;
                        }
                }
                
                return State.Running;
            }
            else{
                return State.Success;
            }
        }
    }

    
}
