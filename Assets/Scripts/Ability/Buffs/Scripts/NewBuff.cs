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
        [SerializeField] private float remainingBuffTime;
        private int stacks = 1;
        public event EventHandler Finished;

        #region Properties

        public BuffScriptableObject BuffSO => buffSO;
        public GameObject Target => target;
        public float RemainingBuffTime => remainingBuffTime;
        public int Stacks => stacks;
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
            remainingBuffTime -= Time.deltaTime;

            // Loop ensures all ticks are processed independent of the frame rate
            // It allows multiple ticks to be processed if the frame rate is lower than the tick rate
            // If tick rate is left at zero or negative, buffs tick action is skipped
            while (timeTillTick <= 0 && buffSO.TickRate > 0)
            {
                // Ensure the buff did not run out before the tick proc'd
                if (remainingBuffTime <= 0 && remainingBuffTime < timeTillTick)
                {
                    break;
                }

                buffSO.Tick(this);
                timeTillTick += buffSO.TickRate;
            }

            if (remainingBuffTime <= 0)
            {
                OnFinish();
            }
        }

        public void Start()
        {
            remainingBuffTime = buffSO.Duration;
            timeTillTick = buffSO.TickRate;
            onHitHooks = buffSO.onHitHooks;
            target.GetComponent<Actor>().OnEffectRecieved.AddListener(OnHitHelperMethod);
            buffSO.StartBuff(this);
        }

        private void OnHitHelperMethod(EffectInstruction _ei)
        {
            // Debug.Log("OnHitHelperMethod");
            onHitHooks.Invoke(this, _ei);
        }

        public void End()
        {
            target.GetComponent<Actor>().OnEffectRecieved.RemoveListener(OnHitHelperMethod);
            onHitHooks.RemoveAllListeners();
            buffSO.EndBuff(this);
        }

        public void AddStack()
        {
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
