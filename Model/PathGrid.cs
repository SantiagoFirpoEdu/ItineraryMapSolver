namespace ItineraryMapSolver.Model;

public struct PathGrid : IGrid<PathNode>
{
	public PathGrid(int width, int height, IntVector endPosition)
	{
		_grid = new Grid<PathNode>(width, height);

		InitializeNodes(endPosition);
	}

	private void InitializeNodes(IntVector endPosition)
	{
		for (int x = 0; x < _grid.Width; x++)
		{
			for (int y = 0; y < _grid.Height; y++)
			{
				ref PathNode pathNode = ref _grid.GetNodeRef(x, y);
				IntVector position = new(x, y);
				pathNode.Index = _grid.ComputeIndex(position);
				pathNode.CostFromStart = PathNode.InitialCostValue;
				pathNode.HeuristicCost = GridMath.GetManhattanDistance(position, endPosition);
				pathNode.InitializeTotalCost();
			}
		}
	}

	public PathNode GetNode(int x, int y)
	{
		return _grid.GetNode(x, y);
	}
	
	public readonly PathNode GetNode(int index)
	{
		return _grid.GetNode(index);
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
	public IntVector Dimensions => _grid.Dimensions;
	public IntVector Destination { get; set; }

	public string DebugPrint()
	{
		return _grid.DebugPrint();
	}
	public HashSet<int> GetNeighbors(IntVector nodePosition)
	{
		return _grid.GetNeighbors(nodePosition);
	}

	public ref PathNode GetNodeRef(int nodeIndex)
	{
		return ref _grid.GetNodeRef(nodeIndex);
	}

	public int ComputeIndex(IntVector position)
	{
		return _grid.ComputeIndex(position);
	}

	public int ComputeIndex(int x, int y)
	{
		return _grid.ComputeIndex(x, y);
	}

	public bool IsValidPosition(IntVector position)
	{
		return _grid.IsValidPosition(position);
	}

	public readonly ref readonly PathNode GetNodeRefReadonly(int nodeIndex)
	{
		return ref _grid.GetNodeRefReadonly(nodeIndex);
	}

	private Grid<PathNode> _grid;
}