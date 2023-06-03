using NUnit.Framework;
using System;
using System.Collections.Generic;
using static TheGameName.EnemyMovementAI;

namespace TheGameName;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> data;

    public int Count { get { return data.Count; } }

    public PriorityQueue()
    {
        data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        HeapifyUp(data.Count - 1);
    }

    public T Dequeue()
    {
        if (Count == 0) throw new Exception("no elements");
        var item = data[0];
        data[0] = data[Count - 1];
        data.RemoveAt(Count - 1);
        HeapifyDown(0);
        return item;
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public bool Any()
    {
        return Count > 0;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            if (data[index].CompareTo(data[parentIndex]) < 0)
            {
                SwapNodes(index, parentIndex);
                index = parentIndex;
            }
            else break;
        }
    }

    private void HeapifyDown(int index)
    {
        while (index < Count / 2)
        {
            var leftChildIndex = index * 2 + 1;
            var rightChildIndex = index * 2 + 2;
            int smallestChildIndex;

            if (rightChildIndex < Count && data[rightChildIndex].CompareTo(data[leftChildIndex]) < 0)
                smallestChildIndex = rightChildIndex;
            else smallestChildIndex = leftChildIndex;

            if (data[index].CompareTo(data[smallestChildIndex]) > 0)
            {
                SwapNodes(index, smallestChildIndex);
                index = smallestChildIndex;
            }
            else break;
        }
    }

    private void SwapNodes(int firstIndex, int secondIndex)
    {
        (data[secondIndex], data[firstIndex]) = (data[firstIndex], data[secondIndex]);
    }
}