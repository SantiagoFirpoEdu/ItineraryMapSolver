namespace ItineraryMapSolver.Model;

public class PathGrid : IGrid<PathNode>
{
    public PathGrid(int width, int height)
    {
        grid = new Grid<PathNode>(width, height);
    }

    public PathNode GetNode(int x, int y)
    {
        throw new NotImplementedException();
    }
    public PathNode SetNode(PathNode newElement, int x, int y)
    {
        PathNode oldNode = grid.GetNode(x, y);
        grid.SetNode(newElement, x, y);
    }
    public int Width => grid.Width;
    public int Height => grid.Height;
    public string DebugPrint()
    {
        throw new NotImplementedException();
    }

    private readonly Grid<PathNode> grid;
}