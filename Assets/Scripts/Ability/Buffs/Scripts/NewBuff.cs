using System;
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
        [SerializeField] private float lastTick = 0.0f;
        [SerializeField] public float remainingTime = 0.0f;
        [SerializeField] private int stacks = 1;
        [SerializeField] public GameObject target;
        public event EventHandler Finished;

        #region Properties

        public int Stacks
        {
            get => stacks;
            set => stacks = value;
        }

        #endregion

        public Buff()
        {
        }

        /// <summary>
        /// Recalculates time values and controls when the buff should tick and end
        /// </summary>
        public void Update()
        {
            if (remainingTime > 0.0f)
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
