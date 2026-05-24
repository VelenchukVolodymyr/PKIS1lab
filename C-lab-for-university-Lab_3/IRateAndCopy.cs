namespace Lab_1
{
    public interface IRateAndCopy
    {
        double Rating { get; }
        object DeepCopy();
    }
}
