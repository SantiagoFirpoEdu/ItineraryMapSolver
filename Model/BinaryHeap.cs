namespace ItineraryMapSolver.Model;

public class BinaryHeap<TElementType> where TElementType : notnull
{
    public BinaryHeap(Comparison<TElementType> comparer)
    {
        _heap = new List<TElementType>();
        _indices = new Dictionary<TElementType, int>();
        this._comparer = comparer;
    }

    public int Count => _heap.Count;

    public void Add(TElementType item)
    {
        _heap.Add(item);
        int currentIndex = _heap.Count - 1;
        _indices[item] = currentIndex;

        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;
            if (_comparer.Invoke(_heap[parentIndex], _heap[currentIndex]) <= 0)
                break;

            Swap(currentIndex, parentIndex);
            currentIndex = parentIndex;
        }
    }

    public TElementType Peek()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("Heap is empty.");

        return _heap[0];
    }

    public TElementType RemoveMin()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("Heap is empty.");

        TElementType minItem = _heap[0];
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _indices[_heap[0]] = 0;
        _heap.RemoveAt(lastIndex);
        _indices.Remove(minItem);

        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = currentIndex * 2 + 1;
            int rightChildIndex = currentIndex * 2 + 2;
            int smallestChildIndex = currentIndex;

            if (leftChildIndex < _heap.Count && _comparer.Invoke(_heap[leftChildIndex], _heap[smallestChildIndex]) < 0)
                smallestChildIndex = leftChildIndex;

            if (rightChildIndex < _heap.Count && _comparer.Invoke(_heap[rightChildIndex], _heap[smallestChildIndex]) < 0)
                smallestChildIndex = rightChildIndex;

            if (smallestChildIndex == currentIndex)
                break;

            Swap(currentIndex, smallestChildIndex);
            currentIndex = smallestChildIndex;
        }

        return minItem;
    }

    public void Remove(TElementType item)
    {
        if (!_indices.ContainsKey(item))
            throw new ArgumentException("Item does not exist in the heap.");

        int indexToRemove = _indices[item];
        int lastIndex = _heap.Count - 1;

        if (indexToRemove == lastIndex)
        {
            _heap.RemoveAt(lastIndex);
            _indices.Remove(item);
            return;
        }

        TElementType itemToSwap = _heap[lastIndex];
        _heap[indexToRemove] = itemToSwap;
        _indices[itemToSwap] = indexToRemove;
        _heap.RemoveAt(lastIndex);
        _indices.Remove(item);

        int parentIndex = (indexToRemove - 1) / 2;

        if (indexToRemove > 0 && _comparer.Invoke(_heap[indexToRemove], _heap[parentIndex]) < 0)
        {
            while (indexToRemove > 0 && _comparer.Invoke(_heap[indexToRemove], _heap[parentIndex]) < 0)
            {
                Swap(indexToRemove, parentIndex);
                indexToRemove = parentIndex;
                parentIndex = (indexToRemove - 1) / 2;
            }
        }
        else
        {
            int currentIndex = indexToRemove;
            while (true)
            {
                int leftChildIndex = currentIndex * 2 + 1;
                int rightChildIndex = currentIndex * 2 + 2;
                int smallestChildIndex = currentIndex;

                if (leftChildIndex < _heap.Count && _comparer.Invoke(_heap[leftChildIndex], _heap[smallestChildIndex]) < 0)
                    smallestChildIndex = leftChildIndex;

                if (rightChildIndex < _heap.Count && _comparer.Invoke(_heap[rightChildIndex], _heap[smallestChildIndex]) < 0)
                    smallestChildIndex = rightChildIndex;

                if (smallestChildIndex == currentIndex)
                    break;

                Swap(currentIndex, smallestChildIndex);
                currentIndex = smallestChildIndex;
            }
        }
    }

    private void Swap(int index1, int index2)
    {
        (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);

        _indices[_heap[index1]] = index1;
        _indices[_heap[index2]] = index2;
    }
    
    private readonly List<TElementType> _heap;
    private readonly Dictionary<TElementType, int> _indices;
    private readonly Comparison<TElementType> _comparer;
}