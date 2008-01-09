using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.UI.Client
{
    class ItemValue
    {
        private short value;
        private object item;

        public ItemValue(short value, object item)
        {
            this.value = value;
            this.item = item;
        }

        public short Value
        {
            get { return value; }
        }

        public object Item
        {
            get { return item; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ItemValue ii = obj as ItemValue;
            if ((object)ii == null)
                return false;

            return this.value == ii.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(ItemValue a, ItemValue b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((object)b);
        }

        public static bool operator !=(ItemValue a, ItemValue b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return item.ToString();
        }
    }
}
