namespace ItineraryMapSolver.Model;

public interface IGrid<TElementType>
{
    public TElementType GetNode(in int x, in int y);
    public TElementType GetNode(in IntVector position);
    public TElementType SetNode(in TElementType newElement, in int x, in int y);
    public int Width { get; }
    public int Height { get; }
    public string DebugPrint();
    Dictionary<IntVector, TElementType> GetNeighbors(in IntVector nodePosition);
}