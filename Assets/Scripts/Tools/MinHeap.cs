
using System;
public class MinHeap
{
    // 2*i : left child 
    // 2*i + 1: right child
    private int[] _keys; 
    private int[] _heap;
    private int _count;
    public int Count { get { return _count; } }

    public MinHeap(int capacity)
    {
        _heap = new int[capacity];
        _keys = new int[capacity];
    }

    #region Tool Methods
    private int parent(int i) { return (i - 1) / 2; }
    private int leftChild(int i) { return 2 * i + 1; }
    private int rightChild(int i) { return 2 * i + 2; }
    private void swap(int i, int j)
    {
        int tmp = _heap[i];
        int keyTmp = _keys[i];
        _heap[i] = _heap[j];
        _keys[i] = _keys[j];
        _heap[j] = tmp;
        _keys[j] = keyTmp;
    }
    private void Up(int i)
    {
        while (i != 0 && _heap[parent(i)] > _heap[i])
        {
            swap(i, parent(i));
            i = parent(i);
        }
    }

    private void Down(int i)
    {
        while (leftChild(i) < _count)
        {
            int iDest = i;
            if (_heap[leftChild(i)] < _heap[i])
            {
                iDest = leftChild(i);
            }
            if (rightChild(i) < _count)
            {
                if(_heap[rightChild(i)] < _heap[i])
                {
                    iDest = (_heap[rightChild(i)] < _heap[iDest]) ? rightChild(i) : iDest;
                }
            }
            // the node is in the correct place
            if (iDest == i) { break; }
            swap(i, iDest);
            i = iDest;
        }
    }
    private void Update(int i, int value)
    {
        if (value > _heap[i])
        {
            _heap[i] = value;
            Down(i);
        }
        else
        {
            _heap[i] = value;
            Up(i);
        }
    }
    #endregion

    #region Methods
    public void Add(int key, int priority)
    {
        _heap[_count] = priority;
        _keys[_count] = key;
        Up(_count);
        _count ++;
    }

    public int TakeMin()
    {
        int res = _keys[0];
        _count --;
        _heap[0] = _heap[_count];
        _keys[0] = _keys[_count];
        Down(0);
        return res;
    }


    public void Update_priority(int key, int new_priority)
    {
        for (int i = 0; i < _count; i ++)
        {
            if (_keys[i] == key)
            {
                Update(i, new_priority);
                break;
            }   
        }
    }

    public int GetPriority(int key)
    {
        for (int i = 0; i < _count; i ++)
        {
            if (_keys[i] == key)
            {
                return _heap[i];
            }
        }

        return Int32.MaxValue;
    }

    public bool IsHere(int key)
    {
        for (int i = 0; i < _count; i ++)
        {
            if (_keys[i] == i) { return true; }
        }
        return false;
    }
    #endregion
}