using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T: IComparable<T>
{
    private List<T> data;

    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int currentIndex = data.Count - 1;
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;
            if (data[currentIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }
            T tmp = data[currentIndex];
            data[currentIndex] = data[parentIndex];
            data[parentIndex] = tmp;
            currentIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (isEmpty())
        {
            throw new Exception("Queue is empty!");
        }
        // Assumes pq isn't empty
        int lastIndex = data.Count - 1;
        T frontItem = data[0];
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        --lastIndex;
        int parentIndex = 0;
        while (true)
        {
            int currentIndex = parentIndex * 2 + 1;
            if (currentIndex > lastIndex) break;
            int rc = currentIndex + 1;
            if (rc <= lastIndex && data[rc].CompareTo(data[currentIndex]) < 0)
                currentIndex = rc;
            if (data[parentIndex].CompareTo(data[currentIndex]) <= 0) break;

            // Swap
            T tmp = data[parentIndex];
            data[parentIndex] = data[currentIndex];
            data[currentIndex] = tmp;

            parentIndex = currentIndex;
        }
        return frontItem;
    }

    public bool isEmpty()
    {
        return data.Count == 0;
    }
}
