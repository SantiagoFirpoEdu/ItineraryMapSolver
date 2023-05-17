namespace ItineraryMapSolver.Model;

public readonly record struct IntVector(int X, int Y)
{
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static IntVector operator -(IntVector first, IntVector second)
    {
        return new IntVector(first.X - second.X, first.Y - second.Y);
    }
    
    public static IntVector operator +(IntVector first, IntVector second)
    {
        return new IntVector(first.X + second.X, first.Y + second.Y);
    }
}