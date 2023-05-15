using System.Text;

namespace ItineraryMapSolver.Model;

public readonly struct Grid<TElementType> : IGrid<TElementType>
{
    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        data = new TElementType[height][];

        InitializeHorizontalAxis();
    }

    public TElementType GetNode(int x, int y)
    {
        return data[FlipY(y)][x];
    }

    public TElementType SetNode(TElementType newElement, int x, int y)
    {
        TElementType oldElement = GetNode(x, y);
        data[FlipY(y)][x] = newElement;
        return oldElement;
    }

    private void InitializeHorizontalAxis()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new TElementType[Width];
        }
    }

    private readonly TElementType[][] data;
    public int Width { get; }
    public int Height { get; }

    public string DebugPrint()
    {
        StringBuilder builder = new StringBuilder(Width * Height);

        foreach (var line in data)
        {
            foreach (TElementType element in line)
            {
                builder.Append(element);
            }

            builder.Append('\n');
        }

        return builder.ToString();
    }

    private int FlipY(int y)
    {
        return data.Length - 1 - y;
    }
}