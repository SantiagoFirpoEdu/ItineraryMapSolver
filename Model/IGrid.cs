namespace ItineraryMapSolver.Model;

public interface IGrid<TElementType> where TElementType : INode<TElementType>
{
    public TElementType GetNode(int x, int y);
    public TElementType GetNode(IntVector position);
    public TElementType SetNode(in TElementType newElement, int x, int y);
    public int Width { get; }
    public int Height { get; }
    public string DebugPrint();
    Dictionary<IntVector, TElementType> GetNeighbors(IntVector nodePosition);
}