// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BuffSystem
{
    /// <summary>
    /// Stores the instance specific data of a buff
    /// </summary>
    [Serializable]
    public class Buff
    {
        [SerializeField] private readonly BuffScriptableObject buffSO;
        [SerializeField] private readonly GameObject target;
        [SerializeField] private float timeTillTick;
        [SerializeField] private float remainingStackTime;
        [SerializeField] private float remainingBuffTime;
        // private readonly Queue<float> stackEndTimes = new();

        /* RR: Made stacks a simple int. They don't run out like the old ones did*/
        private int stacks = 1;
        public event EventHandler Finished;

        #region Properties

        public BuffScriptableObject BuffSO => buffSO;
        public GameObject Target => target;
        public float RemainingBuffTime => remainingBuffTime;
        public int Stacks => stacks;
        // public int Stacks => stackEndTimes.Count + 1;
        [SerializeField] public UnityEvent<Buff, EffectInstruction> onHitHooks;

        #endregion

        public Buff(BuffScriptableObject b, GameObject t)
        {
            buffSO = b;
            target = t;
        }

        /// <summary>
        /// Recalculates time values and controls when the buff should tick and end
        /// </summary>
        public void Update()
        {
            timeTillTick -= Time.deltaTime;
            remainingStackTime -= Time.deltaTime;
            remainingBuffTime -= Time.deltaTime;

            // Loop ensures all ticks are processed independent of the frame rate
            // It allows multiple ticks to be processed if the frame rate is lower than the tick rate
            // If tick rate is left at zero or negative, buffs tick action is skipped
            while (timeTillTick <= 0 && buffSO.TickRate > 0)
            {
                // RemoveExpiredStacksAtTickTime();
                // Ensure the stack did not run out before the tick proc'd
                // if (remainingStackTime > timeTillTick)
                // {
                // }
                buffSO.Tick(this);
                timeTillTick += buffSO.TickRate;
                // Only way to break out of loop if remainingStackTime < timeTillTick. Can't be added to while condition
                // else if (stackEndTimes.Count < 1)
                // {
                //     break;
                // }
            }

            if (remainingBuffTime <= 0)
            {
                OnFinish();
            }
        }

        /// <summary>
        /// Removes stacks from the queue until a stack that has not expired for the current tick is found or the queue is empty
        /// </summary>
        // private void RemoveExpiredStacksAtTickTime()
        // {
        //     if (remainingStackTime > 0 || stackEndTimes.Count < 1)
        //     {
        //         return;
        //     }
        //     while (remainingStackTime <= timeTillTick && stackEndTimes.Count > 0)
        //     {
        //         remainingStackTime = stackEndTimes.Dequeue() - Time.time;
        //         Debug.Log("Stack rm: " + stackEndTimes.Count);
        //     }
        // }

        public void Start()
        {
            remainingStackTime = buffSO.Duration;
            remainingBuffTime = buffSO.Duration;
            timeTillTick = buffSO.TickRate;
            onHitHooks = buffSO.onHitHooks;
            // Debug.Log(target.name);
            target.GetComponent<Actor>().OnEffectRecieved.AddListener(OnHitHelperMethod);
            // onHitHooks.AddListener(OnHitHooksTest);
            buffSO.StartBuff(this);
        }

        private void OnHitHelperMethod(EffectInstruction _ei)
        {
            // Debug.Log("OnHitHelperMethod");
            onHitHooks.Invoke(this, _ei);

        }
        // void OnHitHooksTest(Buff _b, EffectInstruction _ei)
        // {
        //     Debug.Log("OnHitHooks invoked");
        // }

        public void End()
        {
            target.GetComponent<Actor>().OnEffectRecieved.RemoveListener(OnHitHelperMethod);
            onHitHooks.RemoveAllListeners();
            buffSO.EndBuff(this);
        }

        public void AddStack()
        {
            // stackEndTimes.Enqueue(Time.time + buffSO.Duration);

            stacks += 1;
            BuffSO.StacksChanged(this, 1);
            remainingBuffTime = buffSO.Duration;

        }

        public void Refresh()
        {
            remainingBuffTime = buffSO.Duration;
        }

        protected virtual void OnFinish()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}
