using System.Collections.Generic;

public class PriorityQueue
{
    #region Variables
    private MinHeap _heap;
    public int Count { get { return _heap.Count; } }
    #endregion

    public PriorityQueue(int capacity)
    {
        _heap = new MinHeap(capacity);
    }

    public void EnqueueRange(int key, int priority)
    {
        _heap.Add(key, priority);
    }

    public int Dequeue()
    {
        return _heap.TakeMin();
    }

    public void ChangePriority(int key, int new_priority)
    {
        _heap.Update_priority(key, new_priority);
    }

    public int GetPriority(int key)
    {
        return _heap.GetPriority(key);
    }

    public bool IsHere(int key)
    {
        return IsHere(key);
    }
} 