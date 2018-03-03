// StackTraceElement.cs
// (C) 2018 Masato Kokubo

namespace DebugTrace {
	/// <summary>
	/// StackTraceElement struct.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public struct StackTraceElement {
		public StackTraceElement(string typeName, string methodName, string fileName, int lineNumber) {
			TypeName   = typeName  ;
			MethodName = methodName;
			FileName   = fileName  ;
			LineNumber = lineNumber;
		}

		public string TypeName   {get;} // The class name
		public string MethodName {get;} // The method nme
		public string FileName   {get;} // The file name
		public int    LineNumber {get;} // The line number
	}
}
