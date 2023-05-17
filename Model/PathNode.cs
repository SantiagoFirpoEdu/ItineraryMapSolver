namespace ItineraryMapSolver.Model;

public class PathNode : INode<PathNode>
{
    public PathNode(IntVector position, Dictionary<IntVector, PathNode> neighbors)
    {
        Position = position;
        Neighbors = neighbors;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public IntVector? Position { get; }
    public Dictionary<IntVector, PathNode>? Neighbors { get; init; }

    public void ComputeTotalCost()
    {
        _totalCost = _heuristicCost + _distanceFromStart;
    }

    public const int UnitWalkingCost = 1;
    private int _heuristicCost = int.MaxValue;
    private int _distanceFromStart = int.MaxValue;
    private int _totalCost = int.MaxValue;

    public PathNode()
    {
    }
}