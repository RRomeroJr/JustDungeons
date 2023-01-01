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
        public BuffScriptableObject buff;
        [SerializeField] public float lastTick = 0.0f; // time since last tick
        [SerializeField] public float remainingTime = 0.0f;
        [SerializeField] public Actor caster;
        [SerializeField] private IBuff target;
        [SerializeField] private Actor actor;
        [SerializeField] public uint stacks = 1;
        public event EventHandler Finished;

        public Buff(BuffScriptableObject b, IBuff t)
        {
            buff = b;
            target = t;
            remainingTime = buff.Duration;
            buff.StartBuff(target);
        }

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
