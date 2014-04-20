using System;

namespace Foundation {
	sealed class BinaryHeap<T> : IPriorityQueue<T> where T : IComparable<T> {
	
	    private int _currentSize;
		private T[] _array;
	
	    public BinaryHeap(int capacity) 
		{
	        _currentSize = 0;
	        _array = new T[capacity];
	    }
	
	    public void Enqueue(T element) 
		{
	        if (_currentSize == _array.Length - 1) 
			{
	            DoubleArray();
	        }
	    
	        int hole = ++_currentSize;
	        _array[ 0 ] = element;
	        for (; element.CompareTo(_array[hole / 2]) < 0; hole /= 2 ) 
			{
	            _array[hole] = _array[hole / 2];
	        }
	
	        _array[hole] = element;
	    }
	
	    public T Peek() 
		{
	        if (IsEmpty())
	            throw new InvalidOperationException("Queue is empty!");
	        return _array[1];
	    }
	
	    public T Dequeue() 
		{
	        T minItem = Peek();
	        _array[1] = _array[_currentSize--];
	        TrickleDown(1);
	        return minItem;
	    }
	
	    public bool IsEmpty() 
		{
	        return _currentSize == 0;
	    }
	
	    public void Clear() 
		{
	        _currentSize = 0;
	    }
	
	    private void BuildHeap() 
		{
	        for (int i = _currentSize / 2; i > 0; i--) 
	            TrickleDown(i);
	    }
	
	    private void TrickleDown(int hole) 
		{
	        int child;
	        T tmp = _array[hole];
	        
	        bool foundInsertionPoint = false;
	        while (hole * 2 <= _currentSize && !foundInsertionPoint) 
			{
	            child = hole * 2;
	            if (child != _currentSize && _array[child + 1].CompareTo(_array[child]) < 0)
	                child++;
	
	            foundInsertionPoint = _array[child].CompareTo(tmp) >= 0;
	            if (!foundInsertionPoint)
				{
	                _array[hole] = _array[child];
	                hole = child;
	            }
	        }
	        _array[hole] = tmp;
	    }
	
	    private void DoubleArray()
		{
	        T [] newArray;
	    
	        newArray = new T[_array.Length * 2];
	        for (int i = 0; i < _array.Length; i++)
	            newArray[i] = _array[i];
	        _array = newArray;
	    }
	}
}