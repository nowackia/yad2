using System;
using System.Collections.Generic;

namespace AntHill.NET
{
    public class LIList<T> : LinkedList<T>
    {
        public T this[int index]
        {
            get
            {
                LinkedList<T>.Enumerator e = base.GetEnumerator();
                for (int i = 0; i <= index; i++)
                    e.MoveNext();
                return e.Current;
            }
        }
    }
}
