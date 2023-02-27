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
        [SerializeField] public BuffScriptableObject buffSO;
        [SerializeField] private float timeTillTick;
        [SerializeField] private float remainingStackTime;
        [SerializeField] public float remainingBuffTime;
        [SerializeField] public GameObject target;
        private readonly Queue<float> stackEndTimes = new();
        public event EventHandler Finished;

        #region Properties

        public int Stacks => stackEndTimes.Count + 1;

        #endregion

        public Buff()
        {
        }

        /// <summary>
        /// Recalculates time values and controls when the buff should tick and end
        /// </summary>
        public void Update()
        {
            timeTillTick -= Time.deltaTime;
            remainingStackTime -= Time.deltaTime;
            remainingBuffTime -= Time.deltaTime;
            {
                remainingTime -= Time.deltaTime;
                lastTick += Time.deltaTime;
            }

            if (lastTick >= buffSO.TickRate)
            {
                buffSO.Tick(target);
                lastTick -= buffSO.TickRate;
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
            buffSO.StartBuff(target);
            remainingStackTime = buffSO.Duration;
            remainingBuffTime = buffSO.Duration;
            timeTillTick = buffSO.TickRate;
        }

        public void End()
        {
            buffSO.EndBuff(target);
        }

        protected virtual void OnFinish()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}
