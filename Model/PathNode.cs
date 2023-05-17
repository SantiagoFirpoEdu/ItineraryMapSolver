namespace ItineraryMapSolver.Model;

public struct PathNode : INode<PathNode>
{
    public PathNode(IntVector position, Dictionary<IntVector, PathNode> neighbors, int gridWidth)
    {
        Position = position;
        Neighbors = neighbors;
        _index = Grid<PathNode>.ComputeIndex(position, gridWidth);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public IntVector? Position { get; set; }
    public Dictionary<IntVector, PathNode>? Neighbors { get; set; }

    public void ComputeTotalCost()
    {
        TotalCost = _heuristicCost + _distanceFromStart;
    }

    public const int UnitWalkingCost = 1;
    private int _heuristicCost = int.MaxValue;
    private int _distanceFromStart = int.MaxValue;
    private int _index;
    public int TotalCost { get; private set; } = int.MaxValue;

    public PathNode(int index)
    {
        _index = index;
    }

    public PathNode()
    {
    }
}