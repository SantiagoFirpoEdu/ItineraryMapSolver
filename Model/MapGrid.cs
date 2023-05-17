namespace ItineraryMapSolver.Model;

public struct MapGrid : IGrid<MapNode>
{
    public bool AddDestination(int destinationId, int x, int y)
    {
        return _destinations.TryAdd(destinationId, new IntVector(x, y));
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
    public string DebugPrint()
    {
        return _grid.DebugPrint();
    }

    public Dictionary<IntVector, MapNode> GetNeighbors(IntVector nodePosition)
    {
        return _grid.GetNeighbors(nodePosition);
    }

    private readonly Dictionary<int, IntVector> _destinations = new();
    private Grid<MapNode> _grid;
}