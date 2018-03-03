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
	public class VisualBasic : Trace {
		/// An ITrace object for Visual Basic
		public static ITrace Trace {get;} = new VisualBasic();

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
			typeof(char[]  ),
			typeof(byte[]  ),
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
			typeof(char[]  ),
			typeof(byte[]  ),
			typeof(DateTime),
		};

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
		/// Appends the value for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="value">the value object</param>
		/// <param name="isElement">true if the value is element of a container class, false otherwise</param>
		/// 
		protected override void Append(State state, IList<string> strings, StringBuilder buff, object value, bool isElement) {
			if (value == null) {
				buff.Append("Nothing");
			} else {
				var type = value.GetType();

				var typeName = GetTypeName(type, value, isElement);
				if (typeName != null)
					buff.Append(typeName);

				switch (value) {
				case bool         boolValue: buff.Append(boolValue ? "True" : "False"); break;
				case char         charValue: buff.Append('"'); Append(buff, charValue, '\''); buff.Append("\"c"); break;
				case sbyte       sbyteValue: buff.Append(sbyteValue  ); break;
				case byte         byteValue: buff.Append(byteValue   ); break;
				case short       shortValue: buff.Append(shortValue  ).Append('S' ); break;
				case ushort     ushortValue: buff.Append(ushortValue ).Append("US"); break;
				case int           intValue: buff.Append(intValue    ); break;
				case uint         uintValue: buff.Append(uintValue   ).Append('U' ); break;
				case long         longValue: buff.Append(longValue   ).Append('L' ); break;
				case ulong       ulongValue: buff.Append(ulongValue  ).Append("UL"); break;
				case float       floatValue: buff.Append(floatValue  ).Append('F' ); break;
				case double     doubleValue: buff.Append(doubleValue ); break;
				case decimal   decimalValue: buff.Append(decimalValue).Append('D' ); break;
				case DateTime      dateTime: buff.Append(string.Format(dateTimeFormat, dateTime)); break;
				case string     stringValue: Append(buff, stringValue); break;
				case IDictionary dictionary: Append(state, strings, buff, dictionary); break;
				case ICollection collection: Append(state, strings, buff, collection); break;
				case Enum         enumValue: buff.Append(enumValue); break;
				default:
					// Other
				//	bool isReflection = reflectionClasses.Contains(type.FullName); // TODO
					bool isReflection = true;
				//	if (!isReflection && !HasToString(type)) {
				//		isReflection = true;
				//		reflectionClasses.Add(type.FullName);
				//	}

					if (isReflection) {
						// Use Reflection
						if (reflectedObjects.Any(obj => value == obj))
							// Cyclic reference
							buff.Append(cyclicReferenceString).Append(value);
						else {
							// Use Reflection
							reflectedObjects.Add(value);
							AppendReflectString(state, strings, buff, value);
							reflectedObjects.Remove(reflectedObjects.Count - 1);
						}
					} else {
						// Use ToString method
						buff.Append(value);
					}
					break;
				}
			}
		}

		/// <summary>
		/// Returns the type name to be output to the log.<br>
		/// If dose not output, returns null.
		/// </summary>
		///
		/// <param name="type">the type of the value</param>
		/// <param name="value">the value object</param>
		/// <param name="isElement">true if the value is element of a container class, false otherwise</param>
		/// <param name="nest">current nest count</param>
		/// <returns>the type name to be output to the log</returns>
		protected override string GetTypeName(Type type, object value, bool isElement, int nest = 0) {
			string typeName = null;

			if (type.IsArray) {
				// Array
				typeName = GetTypeName(type.GetElementType(), null, false, nest + 1);
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
			} else if (type.Name.StartsWith("Tuple`")) {
				// Tuple<X, Y>(x, y)
				typeName = "Tuple";

			} else if (type.Name.StartsWith("ValueTuple`")) {
				// (x, y)

			} else {
				// Not Array
				var noOutputType = isElement ? NoOutputElementTypes.Contains(type) : NoOutputTypes.Contains(type);
				if (nest > 0 || !noOutputType) {
					// Output the type name
					if (TypeNameMap.ContainsKey(type)) {
						typeName = TypeNameMap[type];
					} else {
						typeName = ReplaceTypeName(type.FullName);
						if (value is ICollection collection)
							typeName += " Count:" + collection.Count;
					}
				}
				if (typeName != null && nest == 0)
					typeName += ' ';
			}

			return typeName;
		}
	}
}
