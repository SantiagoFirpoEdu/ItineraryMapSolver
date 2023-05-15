namespace ItineraryMapSolver.Model;

public readonly struct MapGrid
{
    public MapGrid(int width, int height)
    {
        grid = new Grid<MapNode>(width, height);
    }

    public bool AddDestination(int destinationId, int x, int y)
    {
        return destinations.TryAdd(destinationId, new IntVector(x, y));
    }

    private readonly Grid<MapNode> grid;
    private readonly Dictionary<int, IntVector> destinations = new();

    public void SetNode(MapNode node, int x, int y)
    {
        grid.SetNode(node, x, y);
    }

    public string DebugPrint()
    {
        return grid.DebugPrint();
    }
}