namespace ItineraryMapSolver.Model;

public struct MapGrid : IGrid<MapNode>
{
    public bool AddDestination(int destinationId, int x, int y)
    {
        return Destinations.TryAdd(destinationId, new IntVector(x, y));
    }

    public MapGrid(int width, int height)
    {
        _grid = new Grid<MapNode>(width, height);
    }
    public MapNode GetNode(int x, int y)
    {
        return _grid.GetNode(x, y);
    }
    public MapNode GetNode(IntVector position)
    {
        return _grid.GetNode(position);
    }

    public MapNode SetNode(in MapNode newElement, int x, int y)
    {
        return _grid.SetNode(newElement, x, y);
    }
    public int Width => _grid.Width;
    public int Height => _grid.Height;
    public IntVector Dimensions => _grid.Dimensions;

    public string DebugPrint()
    {
        return _grid.DebugPrint();
    }

    public HashSet<int> GetNeighbors(IntVector nodePosition)
    {
        return _grid.GetNeighbors(nodePosition);
    }

    public Dictionary<int, IntVector> Destinations { get; } = new();
    private Grid<MapNode> _grid;

    public readonly ref MapNode GetNodeRef(int nodeIndex)
    {
        return ref _grid.GetNodeRef(nodeIndex);
    }

    public readonly int ComputeIndex(IntVector position)
    {
        return _grid.ComputeIndex(position);
    }

    public readonly int ComputeIndex(int x, int y)
    {
        return _grid.ComputeIndex(x, y);
    }
}