// CSharp.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTrace {
	/// <summary>
	/// Output suitable for C#.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class CSharp : TraceBase {
		/// An ITrace object for C#
		public static CSharp Trace {get;} = new CSharp();

		// Set of classes that dose not output the type name
		protected override ISet<Type> NoOutputTypes {get;} = new HashSet<Type>() {
			typeof(bool    ),
			typeof(char    ),
			typeof(int     ),
			typeof(uint    ),
			typeof(long    ),
			typeof(ulong   ),
			typeof(float   ),
			typeof(double  ),
			typeof(decimal ),
			typeof(string  ),
			typeof(DateTime),
		};

		// Set of element types of array that dose not output the type name
		protected override ISet<Type> NoOutputElementTypes {get;} = new HashSet<Type>() {
			typeof(bool    ),
			typeof(char    ),
			typeof(sbyte   ),
			typeof(byte    ),
			typeof(short   ),
			typeof(ushort  ),
			typeof(int     ),
			typeof(uint    ),
			typeof(long    ),
			typeof(ulong   ),
			typeof(float   ),
			typeof(double  ),
			typeof(decimal ),
			typeof(string  ),
			typeof(DateTime),
		};

		// Dictionary of type to type name
		protected override IDictionary<Type, string> TypeNameMap {get;} = new Dictionary<Type, string>() {
			{typeof(object ), "object" },
			{typeof(bool   ), "bool"   },
			{typeof(char   ), "char"   },
			{typeof(sbyte  ), "sbyte"  },
			{typeof(byte   ), "byte"   },
			{typeof(short  ), "short"  },
			{typeof(ushort ), "ushort" },
			{typeof(int    ), "int"    },
			{typeof(uint   ), "uint"   },
			{typeof(long   ), "long"   },
			{typeof(ulong  ), "ulong"  },
			{typeof(float  ), "float"  },
			{typeof(double ), "double" },
			{typeof(decimal), "decimal"},
			{typeof(string ), "string" },
		};

		private CSharp() {
		}

		/// <summary>
		/// Returns the type name of the array to be output to the log.<br>
		/// If dose not output, returns null.
		/// </summary>
		///
		/// <param name="type">the type of the value</param>
		/// <param name="value">the value object</param>
		/// <param name="isElement">true if the value is element of a container class, false otherwise</param>
		/// <param name="nest">current nest count</param>
		/// <returns>the type name to be output to the log</returns>
		protected override string GetArrayTypeName(Type type, object value, bool isElement, int nest) {
			string typeName = GetTypeName(type.GetElementType(), null, false, nest + 1);

			if (typeName.Length > 0) {
				var bracket = "[";
				if (value != null)
					bracket += ((Array)value).Length;
				bracket += ']';
				int braIndex = typeName.IndexOf('[');
				if (braIndex < 0)
					braIndex = typeName.Length;
				typeName = typeName.Substring(0, braIndex) + bracket + typeName.Substring(braIndex) + ' ';
			}

			return typeName;
		}

		protected override void Append(LogBuffer buff, bool     value) {buff.Append(value ? "true" : "false");}
		protected override void Append(LogBuffer buff, char     value) {buff.Append('\''); AppendChar(buff, value, '\'', true); buff.Append('\'');}
		protected override void Append(LogBuffer buff, sbyte    value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, byte     value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, short    value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, ushort   value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, int      value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, uint     value) {buff.Append(value).Append('u' );}
		protected override void Append(LogBuffer buff, long     value) {buff.Append(value).Append('L' );}
		protected override void Append(LogBuffer buff, ulong    value) {buff.Append(value).Append("uL");}
		protected override void Append(LogBuffer buff, float    value) {buff.Append(value).Append('f' );}
		protected override void Append(LogBuffer buff, double   value) {buff.Append(value).Append('d' );}
		protected override void Append(LogBuffer buff, decimal  value) {buff.Append(value).Append('m' );}
		protected override void Append(LogBuffer buff, DateTime value) {buff.Append(string.Format(DateTimeFormat, value));}
	}
}
