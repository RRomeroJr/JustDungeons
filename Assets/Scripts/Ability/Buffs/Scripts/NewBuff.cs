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
        private readonly Queue<float> stackEndTimes;
        public event EventHandler Finished;

        #region Properties

        public int Stacks => stackEndTimes.Count + 1;

        #endregion

        public Buff()
        {
            stackEndTimes = new Queue<float>();
            remainingStackTime = buffSO.Duration;
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

            if (remainingTime <= 0)
            {
                OnFinish();
            }
        }

        public void Start()
        {
            buffSO.StartBuff(target);
            remainingTime = buffSO.Duration;
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
