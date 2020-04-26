// VisualBasic.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DebugTrace {
    /// <summary>
    /// Outputs trace information for Visual Vasic.
    /// </summary>
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class VisualBasic : TraceBase {
        /// <summary>
        /// The only <c>VisualBasic</c> object.
        /// </summary>
        public static VisualBasic Trace {get;} = new VisualBasic();

        /// <summary>
        /// Same as <c>VisualBasic</c> property.
        /// Use this property when <c>Imports System.Diagnostics</c>.
        /// </summary>
        /// <since>1.5.2</since>
        public static VisualBasic Trace_ => Trace;

        /// <summary>
        /// Set of classes that dose not output the type name
        /// </summary>
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

        /// <summary>
        /// Set of element types of array that dose not output the type name
        /// </summary>
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

        /// <summary>
        /// Dictionary of type to type name
        /// </summary>
        protected override IDictionary<Type, string> TypeNameMap {get;} = new Dictionary<Type, string>() {
            {typeof(object ), "Object"  },
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
        /// Returns the type name of the array to be output to the log.
        /// If dose not output, returns null.
        /// </summary>
        /// <param name="type">the type of the value</param>
        /// <param name="value">the value object</param>
        /// <param name="isElement">true if the value is element of a container class, false otherwise</param>
        /// <param name="nest">current nest count</param>
        /// <returns>the type name to be output to the log</returns>
        protected override string GetArrayTypeName(Type type, object? value, bool isElement, int nest) {
            string typeName = GetTypeName(type.GetElementType()!, null, false, nest + 1);

            if (typeName.Length > 0) {
                var bracket = "(";
                if (value != null)
                    bracket += "Length: " + ((Array)value).Length;
                bracket += ')';
                int braIndex = typeName.IndexOf('[');
                if (braIndex < 0)
                    braIndex = typeName.Length;
                typeName = typeName.Substring(0, braIndex) + bracket + typeName.Substring(braIndex);
            }

            return typeName;
        }

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, bool value) {buff.Append(value ? "True" : "False");}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, char value) {buff.Append('"'); AppendChar(buff, value, '\'', true); buff.Append("\"c");}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, sbyte value) {buff.Append(value);}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, byte value) {buff.Append(value);}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, short value) {buff.Append(value).Append('S' );}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, ushort value) {buff.Append(value).Append("US");}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, int value) {buff.Append(value);}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, uint value) {buff.Append(value).Append('U' );}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, long value) {buff.Append(value).Append('L' );}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, ulong value) {buff.Append(value).Append("UL");}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, float value) {buff.Append(value).Append('F' );}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, double value) {buff.Append(value);}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, decimal value) {buff.Append(value).Append('D' );}

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected override void Append(LogBuffer buff, DateTime value) {buff.Append(string.Format(DateTimeFormat, value));}

        /// <summary>
        /// Appends the access modifire of the member information to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="memberInfo">the member information</param>
        /// <returns></returns>
        /// <since>1.5.0</since>
        protected override void AppendAccessModifire(LogBuffer buff, MemberInfo memberInfo) {
            switch (memberInfo) {
            case FieldInfo fieldInfo:
                if (!fieldInfo.IsPublic) {
                    if      (fieldInfo.IsPrivate          ) buff.Append("Private ");
                    else if (fieldInfo.IsFamily           ) buff.Append("Protected ");
                    else if (fieldInfo.IsAssembly         ) buff.Append("Friend ");
                    else if (fieldInfo.IsFamilyOrAssembly ) buff.Append("Protected Friend ");
                    else if (fieldInfo.IsFamilyAndAssembly) buff.Append("Private Protected ");
                }
                break;
            case PropertyInfo propertyInfo:
                if (!propertyInfo.GetMethod?.IsPublic ?? false) {
                    if      (propertyInfo.GetMethod?.IsPrivate          ?? false) buff.Append("Private ");
                    else if (propertyInfo.GetMethod?.IsFamily           ?? false) buff.Append("Protected ");
                    else if (propertyInfo.GetMethod?.IsAssembly         ?? false) buff.Append("Friend ");
                    else if (propertyInfo.GetMethod?.IsFamilyOrAssembly ?? false) buff.Append("Protected Friend ");
                    else if (propertyInfo.GetMethod?.IsFamilyAndAssembly?? false) buff.Append("Private Protected ");
                }
                break;
            }
        }
    }
}
