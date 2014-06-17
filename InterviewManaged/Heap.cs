using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public static class TestHeap
    {
        public static void Test()
        {
            int[] a = { 9, 3, 7, 5, 1, 2, 8, 4 };
            Heap<int> heap = new Heap<int>(a);
            heap.Remove(7);
            while (heap.Count > 0)
            {
                Console.WriteLine(heap.Pop());
            }
        }
    }

    // Binary heap implemented with a dynamically resized array
    public class Heap<T> where T : IComparable<T>
    {
        private List<T> _items; // List for dynamic resizing
        private Dictionary<T, int> _itemIndices; // Used for O(1) lookup of an element's index
        public int Capacity { get; private set; }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }
    
        public Heap(IEnumerable<T> items)
        {
            // Copy input into a list as-is
            _items = new List<T>(items);
            
            // Populate the index lookup dictionary
            _itemIndices = new Dictionary<T, int>(_items.Count);
            for (int i = 0; i < _items.Count; ++i)
            {
                _itemIndices.Add(_items[i], i);
            }

            // Heapify
            // Do a level order traversal from the leaves to the root, sifting down from each node.
            // Start from the last parent.
            int index = (_items.Count - 2)/2;
            while (index >= 0)
            {
                SiftDown(index);
                --index;
            }
        }

        private int Parent(int child)
        {
            return (child - 1)/2;
        }

        private int Left(int parent)
        {
            return parent*2 + 1;
        }
    
        private int Right(int parent)
        {
            return parent*2 + 2;
        }

        // Sift the key at the given index down until the heap property is satisfied or it is a leaf
        private void SiftDown(int index)
        {
            while (true)
            {
                // We add new leaves to the tree from left to right
                // Therefore we only need to consider three cases:
                // 1) No left child (implies no right child and therefore leaf)
                // 2) Left child but no right child
                // 3) Left child and right child

                // If we are at a leaf, we are done
                int left = Left(index);
                if (left >= _items.Count)
                    return;

                // If index has a single child
                int right = Right(index);
                if (right >= _items.Count)
                {
                    int cToLeft = _items[index].CompareTo(_items[left]);
                    if (cToLeft > 0)
                    {
                        Swap(index, left);
                        index = left;
                    }
                    else
                        return;
                }

                // If index has two children
                else
                {
                    // If current is greater than left
                    int cToLeft = _items[index].CompareTo(_items[left]);
                    if (cToLeft > 0)
                    {
                        int cLeftToRight = _items[left].CompareTo(_items[right]);
                        // If left is less than or equal to right, swap with left and continue
                        if (cLeftToRight <= 0)
                        {
                            Swap(index, left);
                            index = left;
                        }
                        // If left is greater than right, swap with right and continue
                        else
                        {
                            Swap(index, right);
                            index = right;
                        }
                    }
                    // If current is less than or equal to left
                    else
                    {
                        int cToRight = _items[index].CompareTo(_items[right]);
                        // If current is greater than right, swap with right and continue
                        if (cToRight > 0)
                        {
                            Swap(index, right);
                            index = right;
                        }
                        // Otherwise, no swaps are necessary and we should terminate
                        else
                            return;
                    }
                }
            }
        }

        private void SiftUp(int index)
        {
            while(index > 0)
            {
                int parent = Parent(index);
                int cToParent = _items[index].CompareTo(_items[parent]);
                if (cToParent < 0)
                {
                    Swap(index, parent);
                    index = parent;
                }
                else
                    return;
            }
        }

        private void Swap(int a, int b)
        {
            T tmp;
            tmp = _items[a];
            _items[a] = _items[b];
            _items[b] = tmp;

            // Update the index lookup dictionary
            _itemIndices.Remove(_items[a]);
            _itemIndices.Add(_items[a], a);
            _itemIndices.Remove(_items[b]);
            _itemIndices.Add(_items[b], b);
        }

        public void Add(T item)
        {
            _items.Add(item);
            _itemIndices.Add(item, _items.Count - 1);
            SiftUp(_items.Count - 1);
        }

        public void Remove(T item)
        {
            int index = _itemIndices[item];
            int last = _items.Count - 1;

            // If the item is already the last in the heap, then simply remove it
            if (index == last)
            {
                _itemIndices.Remove(_items[last]);
                _items.RemoveAt(last);
                return;
            }

            // Swap the item with the last in the heap,
            // then remove the last element of the heap.
            Swap(index, last);
            _itemIndices.Remove(_items[last]);
            _items.RemoveAt(last);

            // Sift the displaced element up if it is greater than its parent
            // and down if it is less than its parent.
            int compareToParent = _items[index].CompareTo(_items[Parent(index)]);
            if (compareToParent < 0)
                SiftUp(index);
            else if (compareToParent > 0)
                SiftDown(index);
        }

        public T Pop()
        {
            T returnItem = _items[0];
            Swap(0, _items.Count-1);
            _itemIndices.Remove(_items[_items.Count - 1]);
            _items.RemoveAt(_items.Count - 1);
            SiftDown(0);
            return returnItem;
        }

        public bool Contains(T item)
        {
            return _itemIndices.ContainsKey(item);
        }
    }
}
