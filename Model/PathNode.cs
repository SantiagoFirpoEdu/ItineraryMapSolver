namespace ItineraryMapSolver.Model;

public class PathNode : INode
{
    public int TotalCost => heuristicCost + distanceFromStart;

    public PathNode(IntVector position)
    {
        Position = position;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public IntVector Position { get; }

    private const int unitWalkingCost = 1;
    private int heuristicCost = 0;
    private int distanceFromStart = 0;
}