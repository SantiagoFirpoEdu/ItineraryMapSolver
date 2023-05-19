namespace ItineraryMapSolver.Model;

public interface IGrid<TElementType> where TElementType : INode<TElementType>
{
    public TElementType GetNode(int x, int y);
    public TElementType GetNode(IntVector position);
    public TElementType SetNode(in TElementType newElement, int x, int y);
    public int Width { get; }
    public int Height { get; }
    IntVector Dimensions { get; }
    public string DebugPrint();
    HashSet<int> GetNeighbors(IntVector nodePosition);
    ref TElementType GetNodeRef(int nodeIndex);
    int PositionToIndex(IntVector position);
    int PositionToIndex(int x, int y);
    bool IsValidPosition(IntVector position);
}