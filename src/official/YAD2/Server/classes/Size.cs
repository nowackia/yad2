using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace Server.Classes
{
    [XmlType(TypeName = "Size", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Size
    {

        [XmlElement(ElementName = "X", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __X;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __XSpecified;

        [XmlIgnore]
        public int X
        {
            get { return __X; }
            set { __X = value; __XSpecified = true; }
        }

        [XmlElement(ElementName = "Y", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Y;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __YSpecified;

        [XmlIgnore]
        public int Y
        {
            get { return __Y; }
            set { __Y = value; __YSpecified = true; }
        }

        public Size()
        {
        }
    }

}
