using System.Collections.Generic;

public class PriorityQueue<T>
{
    #region Variables
    private MinHeap<T> _heap;
    public int Count { get { return _heap.Count; } }
    #endregion

    public PriorityQueue(int capacity)
    {
        _heap = new MinHeap<T>(capacity);
    }

    public void EnqueueRange(T key, int priority)
    {
        _heap.Add(key, priority);
    }

    public T Dequeue()
    {
        return _heap.TakeMin();
    }

    public void ChangePriority(T key, int new_priority)
    {
        _heap.Update_priority(key, new_priority);
    }
} 