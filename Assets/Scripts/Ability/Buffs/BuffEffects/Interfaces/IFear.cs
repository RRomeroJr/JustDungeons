public interface IFear : IBuff
{
    public int Feared { get; set; }

    public void ApplyFear();

    public void RemoveFear();
}
