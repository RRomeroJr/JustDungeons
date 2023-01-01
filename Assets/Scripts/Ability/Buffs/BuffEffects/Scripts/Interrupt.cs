namespace BuffSystem
{
    public class Interrupt : BuffEffect
    {
        public override void ApplyEffect(IBuff t)
        {
            var target = t as IInterrupt;
            if (target != null)
            {
                target.Interrupt();
            }
        }
    }
}

