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
        [SerializeField] public BuffScriptableObject buff;
        [SerializeField] private float lastTick = 0.0f;
        [SerializeField] public float remainingTime = 0.0f;
        [SerializeField] public GameObject target;
        public event EventHandler Finished;

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

            if (lastTick >= buff.TickRate)
            {
                buff.Tick(target);
                lastTick -= buff.TickRate;
            }

            if (remainingTime <= 0)
            {
                OnFinish();
            }
        }

        public void Start()
        {
            buff.StartBuff(target);
            remainingTime = buff.Duration;
        }

        public void End()
        {
            buff.EndBuff(target);
        }

        protected void OnFinish()
        {
            var raiseEvent = Finished;
            if (raiseEvent != null)
            {
                raiseEvent(this, EventArgs.Empty);
            }
        }
    }
}
