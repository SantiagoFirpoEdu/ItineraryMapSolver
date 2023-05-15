using System.Text;

namespace ItineraryMapSolver.Model;

public readonly struct Grid<TElementType> where TElementType : struct
{
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
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
            data[i] = new TElementType[width];
        }
    }

    private readonly TElementType[][] data;
    private readonly int width;
    private readonly int height;

    public string DebugPrint()
    {
        StringBuilder builder = new StringBuilder(width * height);

        for (int y = 0; y < data.Length; y++)
        {
            var line = data[FlipY(y)];
            foreach (TElementType element in line)
            {
                builder.Append(element.ToString());
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