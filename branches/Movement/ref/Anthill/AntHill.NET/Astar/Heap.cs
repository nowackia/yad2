using System;
using System.Collections.Generic;
using System.Text;

namespace AntHill.NET.Heap
{
    class Heap  < T > where T : IComparable,IEquality<T>
    {
        static int N = (int)(astar.Astar.max_depth * astar.Astar.max_depth * Math.PI/2);
        private T [] elements;
        private int len;
        private int count = 0;
        public Heap()
        {
            elements = new T[N];
            len = N;
        }

        public int Count { get { return count; } }

        private void UpHeap(int index)
        {
            T tmp;
            while (elements[index].CompareTo(elements[index / 2])>0)
            {
                tmp = elements[index];
                elements[index] = elements[index / 2];
                elements[index / 2] = tmp;
                index = index / 2;
                if (index == 0) return; 
            }
        }

        private void DownHeap(int index)
        {
            int i;
            T tmp;
            while (index <= count)
            {
                if (2 * index + 2 < count)
                {
                    if (elements[2 * index + 1].CompareTo(elements[2 * index + 2])>0)
                    {
                        i = 2 * index + 1;
                    }
                    else
                    {
                        i = 2 * index + 2;
                    }
                }
                else if (2 * index + 1 < count)
                {
                    i = 2 * index + 1;
                }
                else return;
                if (elements[index].CompareTo(elements[i])<0)
                {
                    tmp = elements[index];
                    elements[index] = elements[i];
                    elements[i] = tmp;
                    index = i;
                }
                else return;
            }

        }

        public void Insert(T element)
        {
            
            if (count == len)
            {
                T [] tab = new T[2*len];
                for (int i = 0; i < len; i++)
                {
                    tab[i] = elements[i];
                }
                len *= 2;
                elements = tab;
                //System.Console.Out.WriteLine("reallokacja do {0}", len);
            }
            elements[count++] = element;
            UpHeap(count - 1);
        }
        
        public T GetMax()
        {
            return elements[0];
        }

        public T DeleteMax()
        {
            T elem = elements[0];
            elements[0] = elements[--count];
            DownHeap(0);
            return elem;
        }

        public void Remove(T value)
        {
            for (int i = 0; i < count; i++)
            {
                if (elements[i].EqualTo(value))
                {
                    elements[i] = elements[--count];
                    DownHeap(i);
                    return;
                }
            }
        }


        public T this[int index]
        {
            get { return elements[index]; }
            set
            {
                if (elements[index].CompareTo(value) > 0)
                {
                    elements[index] = value;
                    DownHeap(index);
                }
                else
                {
                    elements[index] = value;
                    UpHeap(index);
                }
            }
        }
	

        public int Contains(T value)
        {
            for (int i = 0; i < count; i++)
            {
                if (elements[i].EqualTo(value))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    interface IEquality<T>
    {
        bool EqualTo(T to);
    }
}
