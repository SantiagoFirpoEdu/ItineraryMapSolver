using System.Text;

namespace ItineraryMapSolver.Model;

public readonly struct Grid<TElementType> : IGrid<TElementType> where TElementType : new()
{
    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _data = new TElementType[height][];

        InitializeHorizontalAxis();
    }

    public TElementType GetNode(in int x, in int y)
    {
        return _data[FlipY(y)][x];
    }
    public TElementType GetNode(in IntVector position)
    {
        return GetNode(position.X, position.Y);
    }

    public TElementType SetNode(in TElementType newElement, in int x, in int y)
    {
        TElementType oldElement = GetNode(x, y);
        _data[FlipY(y)][x] = newElement;
        return oldElement;
    }

    private void InitializeHorizontalAxis()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i] = new TElementType[Width];
        }
    }

    private readonly TElementType[][] _data;
    public int Width { get; }
    public int Height { get; }

    public string DebugPrint()
    {
        StringBuilder builder = new(Width * Height);

        foreach (var line in _data)
        {
            foreach (TElementType element in line)
            {
                builder.Append(element);
            }

            builder.Append('\n');
        }

        return builder.ToString();
    }

    public Dictionary<IntVector, TElementType> GetNeighbors(in IntVector nodePosition)
    {
        Dictionary<IntVector, TElementType> neighbors = new();
        IntVector bottomLeft = nodePosition + new IntVector(-1, -1);
        if (bottomLeft is { X: >= 0, Y: >= 0 })
        {
            neighbors.Add(bottomLeft, GetNode(bottomLeft));
        }

        IntVector bottomRight = nodePosition + new IntVector(1, -1);
        if (bottomRight.X < Width && bottomRight is { Y: >= 0 })
        {
            neighbors.Add(bottomLeft, GetNode(bottomLeft));
        }

        IntVector topLeft = nodePosition + new IntVector(-1, 1);
        if (topLeft is { X: >= 0 } && topLeft.Y < Height)
        {
            neighbors.Add(bottomLeft, GetNode(bottomLeft));
        }

        IntVector topRight = nodePosition + new IntVector(1, 1);
        if (topRight.X < Width && topRight.Y < Height)
        {
            neighbors.Add(bottomLeft, GetNode(bottomLeft));
        }

        return neighbors;
    }

    private int FlipY(int y)
    {
        return _data.Length - 1 - y;
    }

    public void InitializeNodes(in Func<IntVector, TElementType> nodeSupplier)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                IntVector currentPosition = new(x, y);
                SetNode(nodeSupplier.Invoke(currentPosition), x, y);
            }
        }
    }
}