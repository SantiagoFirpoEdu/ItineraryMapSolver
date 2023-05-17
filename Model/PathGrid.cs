namespace ItineraryMapSolver.Model;

public readonly struct PathGrid : IGrid<PathNode>
{
	public PathGrid(int width, int height)
	{
		_grid = new Grid<PathNode>(width, height);
	
		var grid = _grid;
		PathNode NodeSupplier(IntVector position) => new(position, grid.GetNeighbors(position));

		grid.InitializeNodes(NodeSupplier);
	}

	public PathNode GetNode(in int x, in int y)
	{
		return _grid.GetNode(x, y);
	}

	public PathNode GetNode(in IntVector position)
	{
		return _grid.GetNode(position);
	}
	public PathNode SetNode(in PathNode newElement, in int x, in int y)
	{
		return _grid.SetNode(newElement, x, y);
	}
	public int Width => _grid.Width;
	public int Height => _grid.Height;
	public string DebugPrint()
	{
		return _grid.DebugPrint();
	}
	public Dictionary<IntVector, PathNode> GetNeighbors(in IntVector nodePosition)
	{
		return _grid.GetNeighbors(nodePosition);
	}
	
	private readonly Grid<PathNode> _grid;
}