using ItineraryMapSolver.Monads;

namespace ItineraryMapSolver.Model;

public struct PathNode : INode<PathNode>
{
    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public IntVector? Position { get; set; }
    public HashSet<int>? Neighbors { get; set; }

    public void ComputeTotalCost()
    {
        TotalCost = HeuristicCost + CostFromStart;
    }

    public const int UnitWalkingCost = 1;
    public int HeuristicCost { get; set; }
    public int CostFromStart { get; set; }
    public int Index { get; set; }
    public int TotalCost { get; private set; }
    
    public Option<int> CameFromNodeIndex { get; set; }

    public PathNode()
    {
    }

    public void InitializeTotalCost()
    {
        TotalCost = InitialCostValue;
    }
    
	public const int InitialCostValue = int.MaxValue / 2;
}