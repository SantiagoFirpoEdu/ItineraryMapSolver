using ItineraryMapSolver.Monads;

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
    
    public readonly string DebugPrintPath(HashSet<IntVector> path)
    {
        return _grid.DebugPrintPath(path);
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

    public readonly int PositionToIndex(IntVector position)
    {
        return _grid.PositionToIndex(position);
    }

    public readonly int PositionToIndex(int x, int y)
    {
        return _grid.PositionToIndex(x, y);
    }

    public bool IsValidPosition(IntVector position)
    {
        return _grid.IsValidPosition(position);
    }

    public readonly Option<int> GetHarborId(in IntVector position)
    {
        return _grid.GetNode(position).TryGetAsHarbor(out int harborId)
            ? Option<int>.Some(harborId)
            : Option<int>.None();
    }
}