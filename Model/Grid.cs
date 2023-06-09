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
            }
		}
    }
    
    public Grid(int width, int height, Predicate<IntVector> validNodePredicate)
    {
        Width = width;
        Height = height;
        _data = new TElementType[height * width];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
            {
                if (!validNodePredicate.Invoke(new IntVector(x, y)))
                {
                    continue;
                }

                ref TElementType elementType = ref GetNodeRef(x, y);
                elementType.Position = new IntVector(x, y);
            }
		}
    }
    
    public readonly int PositionToIndex(IntVector position)
    {
        return GridMath.PositionToIndex(position.X, position.Y, Width, Height);
    }

    public readonly IntVector IndexToPosition(int index)
    {
        GridMath.IndexToPosition(index, Width, Height, out int x, out int y);
        return new IntVector(x, y);
    }

    public readonly int PositionToIndex(int x, int y)
    {
        return GridMath.PositionToIndex(x, y, Width, Height);
    }
    
    public ref TElementType GetNodeRef(int x, int y)
    {
        return ref _data[PositionToIndex(x, y)];
    }
    
    public readonly ref TElementType GetNodeRefReadonly(int x, int y)
    {
        return ref _data[PositionToIndex(x, y)];
    }
    
    public readonly ref TElementType GetNodeRefReadonly(IntVector position)
    {
        return ref GetNodeRefReadonly(position.X, position.Y);
    }
    
    public readonly TElementType GetNode(int x, int y)
    {
        return _data[PositionToIndex(x, y)];
    }
    public readonly TElementType GetNode(IntVector position)
    {
        return GetNode(position.X, position.Y);
    }

    public TElementType SetNode(in TElementType newElement, int x, int y)
    {
        TElementType oldElement = GetNode(x, y);
        _data[PositionToIndex(x, y)] = newElement;
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

    public readonly HashSet<int> GetNeighbors(IntVector nodePosition, Predicate<IntVector> validNeighborPredicate)
    {
        HashSet<int> neighbors = new();
        IntVector up = nodePosition + new IntVector(0, 1);
        if (up.Y < Height && validNeighborPredicate.Invoke(up))
        {
            neighbors.Add(PositionToIndex(up));
        }

        IntVector down = nodePosition + new IntVector(0, -1);
        if ( down is { Y: >= 0 } && validNeighborPredicate.Invoke(down))
        {
            neighbors.Add(PositionToIndex(down));
        }

        IntVector left = nodePosition + new IntVector(-1, 0);
        if (left is { X: >= 0 } && validNeighborPredicate.Invoke(left))
        {
            neighbors.Add(PositionToIndex(left));
        }

        IntVector right = nodePosition + new IntVector(1, 0);
        if (right.X < Width && validNeighborPredicate.Invoke(right))
        {
            neighbors.Add(PositionToIndex(right));
        }

        return neighbors;
    }
    
    public readonly HashSet<int> GetNeighbors(IntVector nodePosition)
    {
        HashSet<int> neighbors = new();
        IntVector up = nodePosition + new IntVector(0, 1);
        if (up.Y < Height)
        {
            neighbors.Add(PositionToIndex(up));
        }

        IntVector down = nodePosition + new IntVector(0, -1);
        if ( down is { Y: >= 0 })
        {
            neighbors.Add(PositionToIndex(down));
        }

        IntVector left = nodePosition + new IntVector(-1, 0);
        if (left is { X: >= 0 })
        {
            neighbors.Add(PositionToIndex(left));
        }

        IntVector right = nodePosition + new IntVector(1, 0);
        if (right.X < Width)
        {
            neighbors.Add(PositionToIndex(right));
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

    public readonly bool IsValidPosition(IntVector position)
    {
        return position is {X: >= 0, Y: >= 0}
            && position.X < Width
            && position.Y < Height;
    }

    public readonly string DebugPrintPath(HashSet<IntVector> path)
    {
        StringBuilder builder = new(Width * Height);

        for (int index = 0; index < _data.Length; index++)
        {
            IntVector position = IndexToPosition(index);
            ref readonly TElementType element = ref _data[index];
            if (index != 0 && index % Width == 0)
            {
                builder.Append('\n');
            }

            if (path.Contains(position))
            {
                builder.Append('@');
            }
            else
            {
                builder.Append(element);
            }
        }

        return builder.ToString();
    }
}