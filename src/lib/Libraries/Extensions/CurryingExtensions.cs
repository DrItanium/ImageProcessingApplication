//Copyright 2010 Joshua Scoggins. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY Joshua Scoggins ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Joshua Scoggins OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Joshua Scoggins. 
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Libraries.Extensions
{
#if CURRYING
	public static partial class Extensions
	{
		public static Func<R> Curry<T,R>(this Func<T,R> f, T value) 
		{
			return () => f(value); 
		}
		public static Action Curry<T>(this Action<T> a, T value) 
		{
			return () => a(value);
		}
		public static Func<T1,R> Curry<T1,T2,R>(this Func<T1,T2,R> f, T2 val)
		{
			return (x) => f(x,val);
		}
		public static Action<T1> Curry<T1,T2> (this Action<T1,T2> f, T2 val)
		{
			return (x) => f(x,val);
		}
		public static Func<T1,T2,R> Curry<T1,T2,T3,R>(this Func<T1,T2,T3,R> f, T3 value) 
		{
			return (x,y) => f(x,y,value);
		}
		public static Action<T1,T2> Curry<T1,T2,T3>(this Action<T1,T2,T3> f, T3 value) 
		{
			return (x,y) => f(x,y,value);
		}		
		public static Func<T1,T2,T3,R> Curry<T1,T2,T3,T4,R>(this Func<T1,T2,T3,T4,R> f, T4 value) 
		{
			return (x,y,z) => f(x,y,z,value);
		}

		public static Action<T1,T2,T3> Curry<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> f, T4 value)
		{
			return (x,y,z) => f(x,y,z,value);
		}

		//---------Begin Mutliple Currying Functions----
		public static Action<T1,T2> Curry<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> f, T4 val0, T3 val1)
		{
			return (x,y) => f(x,y,val1,val0);
		}
		public static Action<T1> Curry<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> f, T4 val0, T3 val1, T2 val2)
		{
			return (x) => f(x,val2,val1,val0);
		}
		public static Action Curry<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> f, T4 val0, T3 val1, T2 val2, T1 val3)
		{
			return () => f(val3,val2,val1,val0);
		}

		public static Action<T1> Curry<T1,T2,T3>(this Action<T1,T2,T3> f, T3 val0, T2 val1)
		{
			return (x) => f(x,val1,val0);
		}
		public static Action Curry<T1,T2,T3>(this Action<T1,T2,T3> f, T3 val0, T2 val1, T1 val2)
		{
			return () => f(val2,val1,val0);
		}
		public static Action Curry<T1,T2>(this Action<T1,T2> f, T2 val0, T1 val1)
		{
			return () => f(val1, val0);
		}

		public static Func<T1,T2,R> Curry<T1,T2,T3,T4,R>(this Func<T1,T2,T3,T4,R> f, T4 value, T3 value2)
		{
			return (x,y) => f(x,y,value2,value);
		}
		public static Func<T1,R> Curry<T1,T2,T3,T4,R>(this Func<T1,T2,T3,T4,R> f, T4 val0, T3 val1, T2 val2)
		{
			return (x) => f(x,val2,val1,val0);
		}
		public static Func<R> Curry<T1,T2,T3,T4,R>(this Func<T1,T2,T3,T4,R> f, T4 val0, T3 val1, T2 val2, T1 val3)
		{
			return () => f(val3,val2,val1,val0);
		}

		public static Func<T1,R> Curry<T1,T2,T3,R>(this Func<T1,T2,T3,R> fn, T3 val0, T2 val1)
		{
			return (x) => fn(x,val1,val0);
		}
		public static Func<R> Curry<T1,T2,T3,R>(this Func<T1,T2,T3,R> fn, T3 val0, T2 val1, T1 val2)
		{
			return () => fn(val2,val1,val0);
		}
		public static Func<R> Curry<T1,T2,R>(this Func<T1,T2,R> fn, T2 val0, T1 val1)
		{
			return () => fn(val1,val0);
		}
		//begin .NET 4.0 Extensions....ugh Func`17

	}
#endif
}
