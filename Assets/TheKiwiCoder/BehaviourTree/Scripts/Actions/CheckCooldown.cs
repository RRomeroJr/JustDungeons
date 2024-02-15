using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class CheckCooldown : ActionNode
    {
        public Ability_V2 ability;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return BoolToState(context.actor.checkOnCooldown(ability));
        }
    }
}
