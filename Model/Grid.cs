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
    
    public readonly int ComputeIndex(IntVector position)
    {
        return GridMath.ComputeIndex(position.X, position.Y, Width, Height);
    }

    public readonly int ComputeIndex(int x, int y)
    {
        return GridMath.ComputeIndex(x, y, Width, Height);
    }
    
    public ref TElementType GetNodeRef(int x, int y)
    {
        return ref _data[ComputeIndex(x, y)];
    }
    
    public readonly TElementType GetNode(int x, int y)
    {
        return _data[ComputeIndex(x, y)];
    }
    public readonly TElementType GetNode(IntVector position)
    {
        return GetNode(position.X, position.Y);
    }

    public TElementType SetNode(in TElementType newElement, int x, int y)
    {
        TElementType oldElement = GetNode(x, y);
        _data[ComputeIndex(x, y)] = newElement;
        return oldElement;
    }

    private readonly TElementType[] _data;
    public int Width { get; }
    public int Height { get; }
    public IntVector Dimensions => new IntVector(Width, Height);

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

    public readonly HashSet<int> GetNeighbors(IntVector nodePosition)
    {
        HashSet<int> neighbors = new();
        IntVector up = nodePosition + new IntVector(0, 1);
        if (up.Y < Height)
        {
            neighbors.Add(ComputeIndex(up));
        }

        IntVector down = nodePosition + new IntVector(0, -1);
        if ( down is { Y: >= 0 })
        {
            neighbors.Add(ComputeIndex(down));
        }

        IntVector left = nodePosition + new IntVector(-1, 0);
        if (left is { X: >= 0 })
        {
            neighbors.Add(ComputeIndex(left));
        }

        IntVector right = nodePosition + new IntVector(1, 0);
        if (right.X < Width)
        {
            neighbors.Add(ComputeIndex(right));
        }

        return neighbors;
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

    public readonly ref TElementType GetNodeRef(int nodeIndex)
    {
        return ref _data[nodeIndex];
    }

    public readonly TElementType GetNode(int nodeIndex)
    {
        return _data[nodeIndex];
    }

    public readonly ref readonly TElementType GetNodeRefReadonly(int nodeIndex)
    {
        return ref _data[nodeIndex];
    }

    public bool IsValidPosition(IntVector position)
    {
        return position is {X: >= 0, Y: >= 0}
            && position.X < Width
            && position.Y < Height;
    }
}