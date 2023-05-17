namespace ItineraryMapSolver.Model;

public struct PathGrid : IGrid<PathNode>
{
	public PathGrid(int width, int height)
	{
		_grid = new Grid<PathNode>(width, height);
	}

	public PathNode GetNode(int x, int y)
	{
		return _grid.GetNode(x, y);
	}

	public PathNode GetNode(IntVector position)
	{
		return _grid.GetNode(position);
	}
	public PathNode SetNode(in PathNode newElement, int x, int y)
	{
		return _grid.SetNode(newElement, x, y);
	}
	public int Width => _grid.Width;
	public int Height => _grid.Height;
	public string DebugPrint()
	{
		return _grid.DebugPrint();
	}
	public Dictionary<IntVector, PathNode> GetNeighbors(IntVector nodePosition)
	{
		return _grid.GetNeighbors(nodePosition);
	}
	
	private Grid<PathNode> _grid;
}