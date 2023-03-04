using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuffSystem
{
    /// <summary>
    /// Stores the instance specific data of a buff
    /// </summary>
    [Serializable]
    public class Buff
    {
        [SerializeField] private BuffScriptableObject buffSO;
        [SerializeField] private GameObject target;
        [SerializeField] private float timeTillTick;
        [SerializeField] private float remainingStackTime;
        [SerializeField] private float remainingBuffTime;
        private readonly Queue<float> stackEndTimes = new();
        public event EventHandler Finished;

        #region Properties

        public BuffScriptableObject BuffSO => buffSO;
        public GameObject Target => target;
        public float RemainingBuffTime => remainingBuffTime;
        public int Stacks => stackEndTimes.Count + 1;

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
                RemoveExpiredStacksAtTickTime();
                // Ensure the stack did not run out before the tick proc'd
                if (remainingStackTime > timeTillTick)
                {
                    buffSO.Tick(target);
                    timeTillTick += buffSO.TickRate;
                }
                // Only way to break out of loop if remainingStackTime < timeTillTick. Can't be added to while condition
                else if (stackEndTimes.Count < 1)
                {
                    break;
                }
            }

            if (remainingBuffTime <= 0)
            {
                OnFinish();
            }
        }

        /// <summary>
        /// Removes stacks from the queue until a stack that has not expired for the current tick is found or the queue is empty
        /// </summary>
        private void RemoveExpiredStacksAtTickTime()
        {
            if (remainingStackTime > 0 || stackEndTimes.Count < 1)
            {
                return;
            }
            while (remainingStackTime <= timeTillTick && stackEndTimes.Count > 0)
            {
                remainingStackTime = stackEndTimes.Dequeue() - Time.time;
            }
        }

        public void Start()
        {
            remainingStackTime = buffSO.Duration;
            remainingBuffTime = buffSO.Duration;
            timeTillTick = buffSO.TickRate;
            buffSO.StartBuff(target);
        }

        public void End()
        {
            buffSO.EndBuff(target);
        }

        public void AddStack()
        {
            stackEndTimes.Enqueue(Time.time + buffSO.Duration);
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
