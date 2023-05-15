namespace ItineraryMapSolver.Model;

public interface IGrid<TElementType>
{
    public TElementType GetNode(int x, int y);
    public TElementType SetNode(TElementType newElement, int x, int y);
    public int Width { get; }
    public int Height { get; }
    public string DebugPrint();
}