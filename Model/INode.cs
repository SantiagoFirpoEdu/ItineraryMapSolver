namespace ItineraryMapSolver.Model;

public interface INode<TNeighborType>
{
	public IntVector? Position { get; }
	public Dictionary<IntVector, PathNode>? Neighbors { get; protected init; }
}