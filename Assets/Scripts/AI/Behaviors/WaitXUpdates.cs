using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class WaitXUpdates : ActionNode {
        public int NumberOfUpdates = 1;
        int interalNumberOfUpdates;

        protected override void OnStart() {
            interalNumberOfUpdates = NumberOfUpdates;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if(interalNumberOfUpdates <= 0) {
                return State.Success;
            }
            interalNumberOfUpdates -= 1;
            return State.Running;
        }
    }
}
