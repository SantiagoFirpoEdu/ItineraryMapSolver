using ItineraryMapSolver.Model;

namespace ItineraryMapSolver;

public static class GridMath
{
    public static int PositionToIndex(IntVector position, int gridWidth, int gridHeight)
    {
        return PositionToIndex(position.X, position.Y, gridWidth, gridHeight);
    }
    
    public static int PositionToIndex(IntVector position, IntVector gridDimensions)
    {
        return PositionToIndex(position.X, position.Y, gridDimensions.X, gridDimensions.Y);
    }
    
    public static int PositionToIndex(int x, int y, int gridWidth, int gridHeight)
    {
        return x + (gridHeight - y - 1) * gridWidth;
    }
    
    public static void IndexToPosition(int index, int gridWidth, int gridHeight, out int x, out int y)
    {
        y = (gridHeight - 1) - index / gridWidth;
        x = index % gridWidth;
    }
    
    public static IntVector IndexToPosition(int index, int gridWidth, int gridHeight)
    {
        return new IntVector(index % gridWidth, (gridHeight - 1) - index / gridWidth);
    }

    public static int GetManhattanDistance(IntVector from, IntVector to)
    {
        IntVector manhattanDistance = to - from;
        return Math.Abs(manhattanDistance.X) + Math.Abs(manhattanDistance.Y);
    }
}