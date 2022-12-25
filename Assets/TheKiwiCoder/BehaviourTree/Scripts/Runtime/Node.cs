using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public abstract class Node : ScriptableObject {
        public enum State {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Context context;
        [HideInInspector] public Blackboard blackboard;
        [TextArea] public string description;
        public bool drawGizmos = false;

        public State Update() {

            if (!started) {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running) {
                OnStop();
                started = false;
            }

            return state;
        }

        public virtual Node Clone() {
            return Instantiate(this);
        }

        public void Abort() {
            BehaviourTree.Traverse(this, (node) => {
                node.started = false;
                node.state = State.Running;
                node.OnStop();
            });
        }

        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
        public GameObject ContextualTargetToGmObj(HBCTools.ContextualTarget _relTarget){
            MonoBehaviour _comp;
            if(context == null){
                return null;
            }
            if(context.gameObject == null){
                return null;
            }
            switch (_relTarget){
                case HBCTools.ContextualTarget.ArenaObject:
                    _comp = context.gameObject.GetComponent<EnemyController>();
                    if(_comp != null){
                        return (_comp as EnemyController).arenaObject.gameObject;
                    }
                    break;
                case HBCTools.ContextualTarget.Self:
                    return context.gameObject;
                    break;
                case HBCTools.ContextualTarget.Target:
                    _comp = context.gameObject.GetComponent<Actor>();
                    if(_comp != null){
                        return (_comp as Actor).target.gameObject;
                    }
                    break;
                case HBCTools.ContextualTarget.FollowTarget:
                    _comp = context.gameObject.GetComponent<EnemyController>();
                    if(_comp != null){
                        return (_comp as EnemyController).followTarget.gameObject;
                    }
                    break;

            }
            return null;
        }
    }
}