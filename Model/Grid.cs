using System.Text;

namespace ItineraryMapSolver.Model;

public struct Grid<TElementType> : IGrid<TElementType> where TElementType : INode<TElementType>, new()
{
    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _data = new TElementType[height * width];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
            {
                ref TElementType elementType = ref GetNodeRef(x, y);
                elementType.Position = new IntVector(x, y);
                elementType.Neighbors = GetNeighbors((IntVector) elementType.Position);
            }
		}
    }
    
    public static int ComputeIndex(IntVector position, int gridWidth)
    {
        return position.X + FlipY(position.Y, gridWidth) * gridWidth;
    }

    public readonly int ComputeIndex(IntVector position)
    {
        return ComputeIndex(position.X, position.Y, Width);
    }
    
    public readonly int ComputeIndex(int x, int y)
    {
        return ComputeIndex(x, y, Width);
    }
    
    public static int ComputeIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }

    private ref TElementType GetNodeRef(int x, int y)
    {
        return ref _data[ComputeIndex(x, y)];
    }
    
    public readonly TElementType GetNode(int x, int y)
    {
        return _data[ComputeIndex(x, y, Width)];
    }
    public readonly TElementType GetNode(IntVector position)
    {
        return GetNode(position.X, position.Y);
    }

    public TElementType SetNode(in TElementType newElement, int x, int y)
    {
        TElementType oldElement = GetNode(x, y);
        _data[ComputeIndex(x, y, Width)] = newElement;
        return oldElement;
    }

    private readonly TElementType[] _data;
    public int Width { get; }
    public int Height { get; }

    public readonly string DebugPrint()
    {
        StringBuilder builder = new(Width * Height);

        for (int index = 0; index < _data.Length; index++)
        {
            ref readonly TElementType element = ref _data[index];
            if (index != 0 && index % Width == 0)
            {
                builder.Append('\n');
            }
            builder.Append(element);
        }

        return builder.ToString();
    }

    public readonly Dictionary<IntVector, TElementType> GetNeighbors(IntVector nodePosition)
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
            neighbors.Add(bottomRight, GetNode(bottomRight));
        }

        IntVector topLeft = nodePosition + new IntVector(-1, 1);
        if (topLeft is { X: >= 0 } && topLeft.Y < Height)
        {
            neighbors.Add(topLeft, GetNode(topLeft));
        }

        IntVector topRight = nodePosition + new IntVector(1, 1);
        if (topRight.X < Width && topRight.Y < Height)
        {
            neighbors.Add(topRight, GetNode(topRight));
        }

        return neighbors;
    }

    private static int FlipY(int y, int width)
    {
        return width - 1 - y;
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