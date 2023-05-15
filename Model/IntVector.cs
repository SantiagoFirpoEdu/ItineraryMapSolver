namespace ItineraryMapSolver.Model;

public record struct IntVector(int X, int Y)
{
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}