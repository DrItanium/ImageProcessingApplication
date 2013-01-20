#undef NET4
#define NET35
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;
using Libraries.Collections;
using Libraries.LexicalAnalysis;

namespace Libraries.Tycho
{
    public class Keyword : Word, IComparable<Keyword>
    {
        public bool RequiresEqualityCheck { get; set; }
        public Keyword(string input, bool requiresEqualityCheck = false)
            : base(input, input)
        {
            RequiresEqualityCheck = requiresEqualityCheck;
        }
        public override ShakeCondition<string> AsShakeCondition()
        {
            var fn = (RequiresEqualityCheck) ? 
              (LexicalExtensions.GenerateCond<string>(
                        (val, ind, len) => new Tuple<bool, Segment>(val.Equals(TargetWord), new Segment(TargetWord.Length, ind)))) : LexicalExtensions.GenerateMultiCharacterCond(TargetWord);
            return (x) => x.Value.Contains(TargetWord) ? fn(x) : null; //fuck it
        }
        public override TypedShakeCondition<string> AsTypedShakeCondition()
        {
            TypedShakeCondition<string> fn = null;
            if (RequiresEqualityCheck)
            {
                fn = LexicalExtensions.GenerateTypedCond<string>(
                (val, ind, len) =>
                {
                    bool condition = val.Equals(TargetWord);
                    TypedSegment seg = new TypedSegment(TargetWord.Length, WordType, ind);
                    return new Tuple<bool, TypedSegment>(condition, seg);
                } );
            }
            else
            {
                fn = LexicalExtensions.GenerateMultiCharacterTypedCond(TargetWord, TargetWord);
            }
            return (x) =>
                {
                    bool result = x.Value.Contains(TargetWord);
                    return result ? fn(x) : null;  
                };
            //return (x) => x.Value.Contains(TargetWord) ? fn(x) : null;
        }
        public override object Clone()
        {
            return new Keyword(TargetWord, RequiresEqualityCheck);
        }
        public override int GetHashCode()
        {
            return RequiresEqualityCheck.GetHashCode() + base.GetHashCode();
        }
        public virtual int CompareTo(Keyword other)
        {
            return RequiresEqualityCheck.CompareTo(other.RequiresEqualityCheck) 
                + base.CompareTo((Word)other);
        }
        
    }
}
