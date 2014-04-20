using System;

namespace Foundation {
	interface IPriorityQueue<T> where T : IComparable<T> {
	    void Enqueue(T element);
	    T Peek();
	    T Dequeue();
	    bool IsEmpty();
	    void Clear();
	}
}