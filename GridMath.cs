using ItineraryMapSolver.Model;

namespace ItineraryMapSolver;

public class GridMath
{
    public static int ComputeIndex(IntVector position, int gridWidth, int gridHeight)
    {
        return ComputeIndex(position.X, position.Y, gridWidth, gridHeight);
    }
    
    public static int ComputeIndex(int x, int y, int gridWidth, int gridHeight)
    {
        return x + (gridHeight - y - 1) * gridWidth;
    }
}