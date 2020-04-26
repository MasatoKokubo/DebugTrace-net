// StackTraceElement.cs
// (C) 2018 Masato Kokubo

namespace DebugTrace {
    /// <summary>
    /// An element of stack traces.
    /// </summary>
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public struct StackTraceElement {
        /// <summary>
        /// Construct a <c>StackTraceElement</c>.
        /// </summary>
        /// <param name="typeName">the type name</param>
        /// <param name="methodName">the method name</param>
        /// <param name="fileName">the file name</param>
        /// <param name="lineNumber">the line number</param>
        public StackTraceElement(string typeName, string methodName, string fileName, int lineNumber) {
            TypeName   = typeName  ;
            MethodName = methodName;
            FileName   = fileName  ;
            LineNumber = lineNumber;
        }

        /// <summary>
        /// The type name
        /// </summary>
        public string TypeName {get;}

        /// <summary>
        /// The method name
        /// </summary>
        public string MethodName {get;}

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName {get;}

        /// <summary>
        /// The line number
        /// </summary>
        public int LineNumber {get;}
    }
}
