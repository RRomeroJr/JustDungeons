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
        [SerializeField] private BuffScriptableObject buff;
        [SerializeField] private float lastTick = 0.0f;
        [SerializeField] private float remainingTime = 0.0f;
        [SerializeField] private IBuff target;
        public event EventHandler Finished;

        public Buff(BuffScriptableObject b, IBuff t)
        {
            buff = b;
            target = t;
            remainingTime = buff.Duration;
            buff.StartBuff(target);
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
                buff.EndBuff(target);
                OnFinish();
            }
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
