namespace ItineraryMapSolver.Model;

public interface INode<TNeighborType>
{
	public Dictionary<IntVector, TNeighborType>? Neighbors { get;  set; }
	public IntVector? Position { get; set; }
}