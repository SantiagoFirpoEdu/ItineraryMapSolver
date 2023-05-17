namespace ItineraryMapSolver.Model;

public readonly struct MapGrid : IGrid<MapNode>
{
    public bool AddDestination(int destinationId, int x, int y)
    {
        return _destinations.TryAdd(destinationId, new IntVector(x, y));
    }

    public MapGrid(in int width, in int height)
    {
        _grid = new Grid<MapNode>(width, height);

        ref var grid = ref _grid;
    }
    public MapNode GetNode(in int x, in int y)
    {
        return _grid.GetNode(x, y);
    }
    public MapNode GetNode(in IntVector position)
    {
        return _grid.GetNode(position);
    }
    public MapNode SetNode(in MapNode newElement, in int x, in int y)
    {
        return _grid.SetNode(newElement, x, y);
    }
    public int Width => _grid.Width;
    public int Height => _grid.Height;
    public string DebugPrint()
    {
        return _grid.DebugPrint();
    }

    public Dictionary<IntVector, MapNode> GetNeighbors(in IntVector nodePosition)
    {
        return _grid.GetNeighbors(nodePosition);
    }

    private readonly Dictionary<int, IntVector> _destinations = new();
    private readonly Grid<MapNode> _grid;
}