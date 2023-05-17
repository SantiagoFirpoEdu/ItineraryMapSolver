namespace ItineraryMapSolver.Model;

public interface INode<TNeighborType>
{
	public HashSet<int>? Neighbors { get;  set; }
	public IntVector? Position { get; set; }
	int? Index { get; set; }
}