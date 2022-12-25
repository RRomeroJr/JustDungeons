using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class RepeatForXSecs : DecoratorNode {

        public bool restartOnSuccess = true;
        public bool restartOnFailure = true;
        public float duration;
        public float _time;
        public bool endIfChildRunning = false;

        protected override void OnStart() {
            _time = duration;
        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {
            _time -= Time.deltaTime;

            switch (child.Update()) {
                case State.Running:
                    if (endIfChildRunning && _time <= 0.0f) {
                        return State.Success;
                    } else {
                        return State.Running;
                    }
                    break;
                case State.Failure:
                    if (restartOnFailure && _time > 0.0f) {
                        return State.Running;
                    } else {
                        return State.Success;
                    }
                case State.Success:
                    if (restartOnSuccess && _time > 0.0f) {
                        return State.Running;
                    } else {
                        return State.Success;
                    }
            }

            return State.Success;
        }
        
    }

    
}
