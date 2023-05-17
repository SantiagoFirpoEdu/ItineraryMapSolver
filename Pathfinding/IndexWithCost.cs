namespace ItineraryMapSolver.Pathfinding;

public readonly record struct IndexWithCost(int NodeIndex, int TotalCost)
{
    public override int GetHashCode()
    {
        return NodeIndex.GetHashCode();
    }
}