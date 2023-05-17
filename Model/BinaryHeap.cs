namespace ItineraryMapSolver.Model;

public class BinaryHeap<TElementType> where TElementType : notnull
{
    private readonly List<TElementType> heap;
    private readonly IComparer<TElementType> comparer;
    private readonly Dictionary<TElementType, int> itemIndices;

    public BinaryHeap(IComparer<TElementType> comparer)
    {
        this.heap = new List<TElementType>();
        this.comparer = comparer;
        this.itemIndices = new Dictionary<TElementType, int>();
    }

    public BinaryHeap() : this(Comparer<TElementType>.Default)
    {
    }

    public int Count => heap.Count;

    public bool IsEmpty => heap.Count == 0;

    public void Insert(TElementType item)
    {
        heap.Add(item);
        itemIndices[item] = heap.Count - 1;
        PercolateUp(heap.Count - 1);
    }

    public TElementType Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Heap is empty.");

        TElementType item = heap[0];
        itemIndices.Remove(item);

        heap[0] = heap[^1];
        itemIndices[heap[0]] = 0;
        heap.RemoveAt(heap.Count - 1);

        PercolateDown(0);
        return item;
    }

    public bool Remove(TElementType item)
    {
        if (!itemIndices.ContainsKey(item))
            return false;

        int index = itemIndices[item];
        itemIndices.Remove(item);

        if (index == heap.Count - 1)
        {
            heap.RemoveAt(index);
            return true;
        }

        TElementType lastItem = heap[^1];
        heap[index] = lastItem;
        itemIndices[lastItem] = index;
        heap.RemoveAt(heap.Count - 1);

        int parentIndex = (index - 1) / 2;
        if (index > 0 && comparer.Compare(heap[index], heap[parentIndex]) < 0)
            PercolateUp(index);
        else
            PercolateDown(index);

        return true;
    }

    public TElementType GetMax()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Heap is empty.");

        return heap[0];
    }

    public bool Contains(TElementType item)
    {
        return itemIndices.ContainsKey(item);
    }

    private void PercolateUp(int index)
    {
        while (true)
        {
            if (index == 0) return;

            int parentIndex = (index - 1) / 2;

            if (comparer.Compare(heap[index], heap[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                index = parentIndex;
                continue;
            }

            break;
        }
    }

    private void PercolateDown(int index)
    {
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int largestIndex = index;

            if (leftChildIndex < heap.Count && comparer.Compare(heap[leftChildIndex], heap[largestIndex]) > 0) largestIndex = leftChildIndex;

            if (rightChildIndex < heap.Count && comparer.Compare(heap[rightChildIndex], heap[largestIndex]) > 0) largestIndex = rightChildIndex;

            if (largestIndex != index)
            {
                Swap(index, largestIndex);
                index = largestIndex;
                continue;
            }

            break;
        }
    }

    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);

        itemIndices[heap[i]] = i;
        itemIndices[heap[j]] = j;
    }
}