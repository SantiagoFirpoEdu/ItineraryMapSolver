namespace ItineraryMapSolver.Model;

public class PathGrid
{
    public PathGrid(int width, int height)
    {
        grid = new Grid<PathNode>(width, height);
    }

    public int Width => grid.Width;
    public int Height => grid.Height;

    private readonly Grid<PathNode> grid;
}