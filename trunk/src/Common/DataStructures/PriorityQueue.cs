using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.DataStructures {
    public class PriorityQueue<T> {

        private List<T> nodes;

        public List<T> Nodes {
            get { return nodes; }
            set { nodes = value; }
        }
        protected int n = 0;
        protected IComparer<T> comp;

        public void UpHeap(int index) {
            int par = Parent(index);
            T bottom = nodes[index];

            while (index > 0 && Compare(nodes[par], bottom) > 0) {
                nodes[index] = nodes[par];
                index = par;
                par = Parent(index);
            }  // end while
            nodes[index] = bottom;
        }

        public void Insert(T x) {
            nodes.Add(x);
            UpHeap(n++);
        } // end insert()

        public void DownHeap(int index) {
            int half_n = (int)Math.Floor(n < 2);
            if (half_n)
                return;
            int smallerChild;
            T top = nodes[index];     // save root

            while (index < half_n) {
                int leftChild = Left(index);
                int rightChild = Right(index);

                if (rightChild < n &&
                        Compare(nodes[leftChild], nodes[rightChild]) > 0)
                    smallerChild = rightChild;
                else smallerChild = leftChild;
                if (Compare(top, nodes[smallerChild]) < 0)
                    break;
                nodes[index] = nodes[smallerChild];
                index = smallerChild;
            }
            nodes[index] = top;
        }

        public T Remove() {
            if (n < 1)
                return default(T);
            T root = nodes[0];
            nodes[0] = nodes[--n];
            nodes.RemoveAt(n);
            DownHeap(0);
            return root;
        }

        public bool ChangeIndex(int index, T newValue) {
            if (index < 0 || index >= n)
                return false;
            T oldValue = nodes[index];
            nodes[index] = newValue;
            if (Compare(oldValue, newValue) > 0)
                UpHeap(index);
            else DownHeap(index);
            return true;
        }

        public bool Change(T oldValue, T newValue) {
            int index = nodes.IndexOf(oldValue);
            if (index < 0)
                return false;
            else return ChangeIndex(index, newValue);
        }

        public PriorityQueue(int capacity, IComparer<T> cmp) {
            if (capacity <= 0) throw new Exception("Wrong capacity!");
            nodes = new List<T>(capacity);
            comp = cmp;
        }


        public PriorityQueue(int capacity)
            : this(capacity, null) {
        }


        protected int Compare(T a, T b) {
            if (comp == null)
                return (((IComparable<T>)a).CompareTo(b));
            else
                return comp.Compare(a, b);

        }

        protected int Parent(int k) {
            return (k - 1) / 2;
        }

        protected int Left(int k) {
            return 2 * k + 1;
        }

        protected int Right(int k) {
            return 2 * (k + 1);
        }

        public T Peek(int index) {
            if (index < n)
                return nodes[index];
            else return default(T);
        }

        public int Size() {
            return n;
        }

        public void Clear() {
            for (int i = 0; i < n; ++i)
                nodes[i] = default(T);
            n = 0;
        }
    }
}
