// VisualBasic.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTrace {
	/// <summary>
	/// Output suitable for Visual Vasic.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class VisualBasic : TraceBase {
		/// An ITrace object for Visual Basic
		public static VisualBasic Trace {get;} = new VisualBasic();

		// Set of classes that dose not output the type name
		protected override ISet<Type> NoOutputTypes {get;} = new HashSet<Type>() {
			typeof(bool    ),
			typeof(char    ),
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

		// Dictionary of thread id to indent state
		protected override IDictionary<Type, string> TypeNameMap {get;} = new Dictionary<Type, string>() {
			{typeof(bool   ), "Boolean" },
			{typeof(char   ), "Char"    },
			{typeof(sbyte  ), "SByte"   },
			{typeof(byte   ), "Byte"    },
			{typeof(short  ), "Short"   },
			{typeof(ushort ), "UShort"  },
			{typeof(int    ), "Integer" },
			{typeof(uint   ), "UInteger"},
			{typeof(long   ), "Long"    },
			{typeof(ulong  ), "ULong"   },
			{typeof(float  ), "Single"  },
			{typeof(double ), "Double"  },
			{typeof(decimal), "Decimal" },
			{typeof(string ), "String"  },
		};

		private VisualBasic() {
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

			if (typeName != null) {
				var bracket = "(";
				if (value != null)
					bracket += "Length:" + ((Array)value).Length;
				bracket += ')';
				int braIndex = typeName.IndexOf('[');
				if (braIndex < 0)
					braIndex = typeName.Length;
				typeName = typeName.Substring(0, braIndex) + bracket + typeName.Substring(braIndex) + ' ';
			}

			return typeName;
		}

		protected override void Append(LogBuffer buff, bool     value) {buff.Append(value ? "True" : "False");}
		protected override void Append(LogBuffer buff, char     value) {buff.Append('"'); AppendChar(buff, value, '\'', true); buff.Append("\"c");}
		protected override void Append(LogBuffer buff, sbyte    value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, byte     value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, short    value) {buff.Append(value).Append('S' );}
		protected override void Append(LogBuffer buff, ushort   value) {buff.Append(value).Append("US");}
		protected override void Append(LogBuffer buff, int      value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, uint     value) {buff.Append(value).Append('U' );}
		protected override void Append(LogBuffer buff, long     value) {buff.Append(value).Append('L' );}
		protected override void Append(LogBuffer buff, ulong    value) {buff.Append(value).Append("UL");}
		protected override void Append(LogBuffer buff, float    value) {buff.Append(value).Append('F' );}
		protected override void Append(LogBuffer buff, double   value) {buff.Append(value);}
		protected override void Append(LogBuffer buff, decimal  value) {buff.Append(value).Append('D' );}
		protected override void Append(LogBuffer buff, DateTime value) {buff.Append(string.Format(DateTimeFormat, value));}
	}
}
