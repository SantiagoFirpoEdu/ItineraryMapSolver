namespace ItineraryMapSolver.Model;

public struct Grid<TElementType> where TElementType : struct
{
    public Grid(int width, int height)
    {
        data = new TElementType[height][];

        InitializeHorizontalAxis(width);
    }

    public TElementType GetNode(int x, int y)
    {
        return data[y][x];
    }

    public TElementType SetNode(TElementType newElement, int x, int y)
    {
        TElementType oldElement = GetNode(x, y);
        data[y][x] = newElement;
        return oldElement;
    }

    private void InitializeHorizontalAxis(int width)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new TElementType[width];
        }
    }

    private readonly TElementType[][] data;
}