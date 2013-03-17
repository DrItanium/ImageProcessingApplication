using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libraries.Collections;

namespace Libraries.LexicalAnalysis
{
    public class TypedSegment : Segment
    {
        public string Type { get; set; }
        public TypedSegment(int length, string type, int offset)
            : base(length, offset)
        {
            Type = type;
        }
        public TypedSegment(int length, string type)
            : this(length, type, 0)
        {
        }
        public TypedSegment(int length)
        : this(length, string.Empty)
        {
        }
        

        public TypedSegment(TypedSegment seg)
            : base(seg)
        {
            Type = seg.Type;
        }
        public TypedSegment(Segment seg, string type)
            : base(seg)
        {
            Type = type;
        }

        public override object Clone()
        {
            return new TypedSegment(this);
        }

        public override bool Equals(object other)
        {
            TypedSegment seg = (TypedSegment)other;
            return seg.Type.Equals(Type) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Type.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}:{1}=>{2}]", Type, Start, Length);
        }       
    }
}
