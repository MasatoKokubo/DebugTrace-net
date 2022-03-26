The following are examples of C# and Visual Basic source used DebugTrace-net methods and the log of when it has been executed.

&emsp;&emsp;_ReadmeExample.cs_
```c#
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp; // ToDo: Remove after debugging

namespace Readme;

/// <summary>ReadmeExample1</summary>
[TestClass]
public class ReadmeExample1 {
    /// <summary>Example1</summary>
    [TestMethod]
    public void Example1() {
        Trace.Enter(); // ToDo: Remove after debugging

        var contacts = new [] {
            new Contact(1, "Akane" , "Apple", new DateTime(1991, 2, 3)),
            new Contact(2, "Yukari", "Apple", new DateTime(1992, 3, 4))
        };
        Trace.Print("contacts", contacts); // ToDo: Remove after debugging

        Trace.Leave(); // ToDo: Remove after debugging
    }

}

/// <summary>Entity</summary>
public class Entity {
    public int ID;

    public Entity(int id) {
        ID = id;
    }
}

/// <summary>ContactBase</summary>
public class ContactBase : Entity {
    public string FirstName;
    public string LastName;

    public ContactBase(int id, string firstName, string lastName) : base(id) {
        FirstName = firstName; LastName  = lastName ;
    }
}

/// <summary>Contact</summary>
public class Contact : ContactBase {
    public DateTime Birthday;

    public Contact(int id, string firstName, string lastName, DateTime birthday) : base(id, firstName, lastName) {
        Birthday  = birthday ;
    }
}
```

&emsp;&emsp;_Console output / C#_
```log
2022-11-13 13:37:54.163 [05] DebugTrace-net 2.1.0 on .NET Core 3.1.31
2022-11-13 13:37:54.164 [05]   properties file path: Z:\Develop\C#\DebugTrace\DebugTrace\DebugTraceTest\bin\Debug\netcoreapp3.1\DebugTrace.properties
2022-11-13 13:37:54.164 [05]   logger: DebugTrace.Console+Error
2022-11-13 13:37:54.165 [05] 
2022-11-13 13:37:54.165 [05] ______________________________ Thread 5 ______________________________
2022-11-13 13:37:54.165 [05] 
2022-11-13 13:37:54.197 [05] Enter Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:16)
2022-11-13 13:37:54.207 [05] | 
2022-11-13 13:37:54.207 [05] | contacts = Readme.Contact[2] {
2022-11-13 13:37:54.207 [05] |   Readme.Contact {
2022-11-13 13:37:54.207 [05] |     ____ Readme.Entity ____
2022-11-13 13:37:54.207 [05] |     ID: 1
2022-11-13 13:37:54.207 [05] |     ____ Readme.ContactBase ____
2022-11-13 13:37:54.207 [05] |     FirstName: (Length:5)"Akane", LastName: (Length:5)"Apple"
2022-11-13 13:37:54.207 [05] |     ____ Readme.Contact ____
2022-11-13 13:37:54.207 [05] |     Birthday: 1991-02-03 00:00:00.0000000
2022-11-13 13:37:54.207 [05] |   },
2022-11-13 13:37:54.207 [05] |   Readme.Contact {
2022-11-13 13:37:54.207 [05] |     ____ Readme.Entity ____
2022-11-13 13:37:54.207 [05] |     ID: 2
2022-11-13 13:37:54.207 [05] |     ____ Readme.ContactBase ____
2022-11-13 13:37:54.207 [05] |     FirstName: (Length:6)"Yukari", LastName: (Length:5)"Apple"
2022-11-13 13:37:54.207 [05] |     ____ Readme.Contact ____
2022-11-13 13:37:54.207 [05] |     Birthday: 1992-03-04 00:00:00.0000000
2022-11-13 13:37:54.207 [05] |   }
2022-11-13 13:37:54.207 [05] | } (ReadmeExamples.cs:22)
2022-11-13 13:37:54.207 [05] | 
2022-11-13 13:37:54.208 [05] Leave Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:24) duration: 00:00:00.0097074
```

&emsp;&emsp;_ReadmeExample.vb (version 2.1.0)_
```vb
Imports System.Threading
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DebugTrace.VisualBasic ' for Debugging

Namespace Readme
    ''' <summary>ReadmeExample1</summary>
    <TestClass()>
    Public Class ReadmeExample1
        ''' <summary>Example1</summary>
        <TestMethod()>
        Public Sub Example1()
            Trace.Enter() ' for Debugging

            Dim contacts = New Contact() {
                New Contact(1, "Akane", "Apple", New DateTime(1991, 2, 3)),
                New Contact(2, "Yukari", "Apple", New DateTime(1992, 3, 4))
            }
            Trace.Print("contacts", contacts) ' for Debugging

            Trace.Leave() ' for Debugging
        End Sub
    End Class

    ''' <summary>Entity</summary>
    Public class Entity
        Public Property Id As Integer

        Public Sub New(id_ As Integer)
            Id = id_
        End Sub
    End Class

    ''' <summary>ContactBase</summary>
    public class ContactBase : Inherits Entity
        Public Property FirstName As String
        Public Property LastName As String

        Public Sub New(id_ As Integer, firstName_ As String, lastName_ As String)
            MyBase.New(id_)
            FirstName = firstName_ : LastName = lastName_
        End Sub
    End Class

    ''' <summary>Contact</summary>
    Public Class Contact : Inherits ContactBase
        Public Birthday As DateTime

        Public Sub New(id_ As Integer, firstName_ As String, lastName_ As String, birthday_ As DateTime)
            MyBase.New(id_, firstName_, lastName_)
            Birthday = birthday_
        End Sub
    End Class
End Namespace
```

&emsp;&emsp;_Console output / Visual Basic_
```log
2022-11-13 13:40:20.838 [04] DebugTrace-net 2.1.0 on .NET Core 3.1.31
2022-11-13 13:40:20.839 [04]   properties file path: Z:\Develop\C#\DebugTrace\DebugTrace\DebugTraceVBTest\bin\Debug\netcoreapp3.1\DebugTrace.properties
2022-11-13 13:40:20.839 [04]   logger: DebugTrace.Console+Error
2022-11-13 13:40:20.840 [04] 
2022-11-13 13:40:20.840 [04] ______________________________ Thread 4 ______________________________
2022-11-13 13:40:20.840 [04] 
2022-11-13 13:40:20.872 [04] Enter DebugTraceVBTest.Readme.ReadmeExample1.Example1() (ReadmeExamples.vb:12)
2022-11-13 13:40:20.881 [04] | 
2022-11-13 13:40:20.881 [04] | contacts = DebugTraceVBTest.Readme.Contact(Length: 2) {
2022-11-13 13:40:20.881 [04] |   DebugTraceVBTest.Readme.Contact {
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.Entity ____
2022-11-13 13:40:20.881 [04] |     Id: 1
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.ContactBase ____
2022-11-13 13:40:20.881 [04] |     FirstName: (Length:5)"Akane", LastName: (Length:5)"Apple"
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.Contact ____
2022-11-13 13:40:20.881 [04] |     Birthday: 1991-02-03 00:00:00.0000000
2022-11-13 13:40:20.881 [04] |   },
2022-11-13 13:40:20.881 [04] |   DebugTraceVBTest.Readme.Contact {
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.Entity ____
2022-11-13 13:40:20.881 [04] |     Id: 2
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.ContactBase ____
2022-11-13 13:40:20.881 [04] |     FirstName: (Length:6)"Yukari", LastName: (Length:5)"Apple"
2022-11-13 13:40:20.881 [04] |     ____ DebugTraceVBTest.Readme.Contact ____
2022-11-13 13:40:20.881 [04] |     Birthday: 1992-03-04 00:00:00.0000000
2022-11-13 13:40:20.881 [04] |   }
2022-11-13 13:40:20.881 [04] | } (ReadmeExamples.vb:18)
2022-11-13 13:40:20.882 [04] | 
2022-11-13 13:40:20.882 [04] Leave DebugTraceVBTest.Readme.ReadmeExample1.Example1() (ReadmeExamples.vb:20) duration: 00:00:00.0096556
```

### 3.1 When using or Imports System.Diagnostics

If you are `using System.Diagnostics` or `Impors System.Diagnostics`, since the `DebugTrace.CSharp.Trace` (`DebugTrace.VisualBaisc.Trace`) property and `System.Diagnostics.Trace` class overlap, use `Trace_`  property instead of `Trace`.

&emsp;&emsp;_ReadmeExample.cs_
```c#
using System.Diagnostics;
using static DebugTrace.CSharp; // ToDo: Remove after debugging

namespace Readme {
    public class ReadmeExample {
        public static void Main(string[] args) {
            Trace_.Enter(); // ToDo: Remove after debugging
```

&emsp;&emsp;_ReadmeExample.vb_
```vb
Imports System.Diagnostics
Imports DebugTrace.VisualBasic ' ToDo: Remove after debugging

Namespace Global.Readme
    Public Class ReadmeExample
        Public Shared Sub Main(args As String())
            Trace_.Enter() ' ToDo: Remove after debugging
```

## 4. Interfaces and Classes

There are mainly the following interfaces and classes.

&emsp;&emsp;_Interfaces and Classes_
<table>
    <tr><th>Name</th><th>Super Class or Implemented Interfaces</th><th>Description</th></tr>
    <tr><td>DebugTrace.ITrace       </td><td>None              </td><td>Trace processing interface</td></tr>
    <tr><td>DebugTrace.TraceBase    </td><td>DebugTrace.ITrace </td><td>Trace processing base class</td></tr>
    <tr><td>DebugTrace.CSharp       </td><td>DebugTrace.Trace  </td><td>Trace processing class for C#</td></tr>
    <tr><td>DebugTrace.VisualBasic  </td><td>DebugTrace.Trace  </td><td>Trace processing class for VisualBasic</td></tr>
    <tr><td>DebugTrace.ILogger      </td><td>None              </td><td>Log output interface</td></tr>
    <tr><td>DebugTrace.Console      </td><td>DebugTrace.ILogger</td><td>Abstract super class of <code>DebugTrace.Console.Out</code> and <code>DebugTrace.Console.Error</code></td></tr>
    <tr><td>DebugTrace.Console.Out  </td><td>DebugTrace.Console</td><td>Class that outputs logs to standard output</td></tr>
    <tr><td>DebugTrace.Console.Error</td><td>DebugTrace.Console</td><td>Class that outputs logs to standard error output</td></tr>
    <tr><td>DebugTrace.Diagnostics<br>&emsp;<i>since 1.6.0</i></td><td>DebugTrace.ILogger</td><td>Abstract super class of DebugTrace.Diagnostics.Debug and DebugTrace.Diagnostics.Trace class</td></tr>
    <tr><td>DebugTrace.Diagnostics.Debug<br>&emsp;<i>since 1.6.0</i></td><td>DebugTrace.Diagnostics</td><td>Class that outputs logs using <code>System.Diagnostics.Debug</code> class</td></tr>
    <tr><td>DebugTrace.Diagnostics.Trace<br>&emsp;<i>since 1.6.0</i></td><td>DebugTrace.Diagnostics</td><td>Class that outputs logs using <code>System.Diagnostics.Trace</code> class</td></tr>
</table>

## 5. Properties of DebugTrace.CSharp class and DebugTrace.VisualBasic class

`DebugTrace.CSharp` and `DebugTrace.VisualBasic` class has `Trace` and `Trace_` property as an instance of its own type.

## 6. Properties and methods of ITrace interface

It has the following properties and methods.

&emsp;&emsp;_Table of Properties_
<table>
    <tr><th>Name</th><th>Description</th></tr>
    <tr>
        <td>IsEnabled</td>
        <td><code>true</code> if log output is enabled, <code>false</code> otherwise (<code>get</code> only)</td>
    </tr>
    <tr>
        <td>LastLog</td>
        <td>Last log string outputted (get only)</td>
    </tr>
</table>

&emsp;&emsp;_Table of Methods_
<table>
    <tr><th>Name</th><th>DebugTrace<br>version</th><th>Arguments</th><th>Return Value</th><th>Description</th></tr>
    <tr>
        <td><code>ResetNest</code></td>
        <td></td>
        <td><i>None</i></td>
        <td><i>None</i></td>
        <td>Initializes the nesting level for the current thread.</td>
    </tr>
    <tr>
        <td><code>Enter</code></td>
        <td></td>
        <td><i>None</i></td>
        <td><code>int</code> thread ID</td>
        <td>Outputs method start to log.</td>
    </tr>
    <tr>
        <td><code>Leave</code></td>
        <td></td>
        <td><code>int threadId</code>: the thread ID<br>(default: <code>-1</code>)</td>
        <td><i>None</i></td>
        <td>Outputs method end to the log.</td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 or lator</td>
        <td rowspan=2><code>string message</code>: the message</td>
        <td>the message</td>
        <td rowspan=2>Outputs the message to the log.</td>
    </tr>
    <tr>
        <td>2.0.3 or earlier</td>
        <td><i>None</i></td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 or lator</td>
        <td rowspan=2><code>Func&lt;string&gt; messageSupplier</code>: the function to return a message</td>
        <td>the message obtained from <code>messageSupplier</code></td>
        <td rowspan=2>Gets a message from <code>messageSupplier</code> and output it to the log.</td>
    </tr>
    <tr>
        <td>2.0.3 or earlier</td>
        <td><i>None</i></td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 or lator</td>
        <td rowspan=2>
            <code>string name</code>: the value name<br>
            <code>object value</code>: the value
        </td>
        <td>the value</td>
        <td rowspan=2>Outputs to the log in the form of <code>name = value</code></td>
    </tr>
    <tr>
        <td>2.0.3 or earlier</td>
        <td><i>None</i></td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 or lator</td>
        <td rowspan=2>
            <code>string name</code>: the name of the value<br>
            <code>Func&lt;object&gt; valueSupplier</code>: the function to return a value
        </td>
        <td>the value obtained from <code>valueSupplier</code></td>
        <td rowspan=2>
            Gets a value from <code>valueSupplier</code> and output it to the log in the form of
            <code>name = value</code>.
        </td>
    </tr>
    <tr>
        <td>2.0.3 or earlier</td>
        <td><i>None</i></td>
    </tr>
    <tr>
        <td><code>PrintStack</code></td>
        <td>since 1.6.0</td>
        <td><code>int maxCount</code>: maximum number of stack elements to output</td>
        <td><i>None</i></td>
        <td>Outputs call stack to log.</td>
    </tr>
</table>

## 7. Properties of *DebugTrace.properties* file

DebugTrace reads the `DebugTrace.properties` file in the current directory at startup.
You can specify following properties in the `DebugTrace.properties` file.

&emsp;&emsp;_Table of DebugTrace.properties_
<table>
    <tr><th>Property Name</th><th>Description</th></tr>
    <tr>
        <td><code>Logger</code></td>
        <td>
            Logger used by DebugTrace<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>Logger = Console+Out</code> - <i>Outputs to the console (stdout)</i><br>
            &emsp;&emsp;<code>Logger = Console+Error</code> - <i>Outputs to the console (stderr) <i>(This is the defalut)</i></i><br>
            &emsp;&emsp;<code>Logger = Diagnostics+Debug</code> - <i>Outputs using <code>System.Diagnostics.Debug</code> class - since 1.6.0</i><br>
            &emsp;&emsp;<code>Logger = Diagnostics+Trace</code> - <i>Outputs using <code>System.Diagnostics.Trace</code> class - since 1.6.0</i><br>
            &emsp;&emsp;<code>Logger = Log4net</code> - <i>Outputs using <code>Log4net</code></i><br>
            &emsp;&emsp;<code>Logger = NLog</code> - <i>Outputs using <code>NLog</code></i><br>
            <b>Example for multiple outputs:</b> - <i>since 1.5.0</i><br>
            &emsp;&emsp;<code>Logger = Console+Out; Log4net</code> - <i>Outputs to the console (stdout) and using <code>Log4net</code></i>
        </td>
    </tr>
    <tr>
        <td><code>LogLevel</code></td>
        <td>
            Log level to use when outputting<br>
            <b>Examples when using Log4net:</b><br>
            &emsp;&emsp;<code>LogLevel = All</code><br>
            &emsp;&emsp;<code>LogLevel = Finest</code><br>
            &emsp;&emsp;<code>LogLevel = Verbose</code><br>
            &emsp;&emsp;<code>LogLevel = Finer</code><br>
            &emsp;&emsp;<code>LogLevel = Trace</code><br>
            &emsp;&emsp;<code>LogLevel = Fine</code><br>
            &emsp;&emsp;<code>LogLevel = Debug</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;<code>LogLevel = Info</code><br>
            &emsp;&emsp;<code>LogLevel = Notice</code><br>
            &emsp;&emsp;<code>LogLevel = Warn</code><br>
            &emsp;&emsp;<code>LogLevel = Error</code><br>
            &emsp;&emsp;<code>LogLevel = Severe</code><br>
            &emsp;&emsp;<code>LogLevel = Critical</code><br>
            &emsp;&emsp;<code>LogLevel = Alert</code><br>
            &emsp;&emsp;<code>LogLevel = Fatal</code><br>
            &emsp;&emsp;<code>LogLevel = Emergency</code><br>
            &emsp;&emsp;<code>LogLevel = Off</code><br>
            <b>Examples when using NLog:</b><br>
            &emsp;&emsp;<code>LogLevel = Trace</code><br>
            &emsp;&emsp;<code>LogLevel = Debug</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;<code>LogLevel = Info</code><br>
            &emsp;&emsp;<code>LogLevel = Warn</code><br>
            &emsp;&emsp;<code>LogLevel = Error</code><br>
            &emsp;&emsp;<code>LogLevel = Fatal</code><br>
            &emsp;&emsp;<code>LogLevel = Off</code><br>
            <b>Examples when using Log4net and NLog:</b> (Logger = Log4net; NLog)<br>
            &emsp;&emsp;<code>LogLevel = Debug</code> - <i>Outputs <code>Debug</code> level for both <code>Log4net</code> and <code>NLog</code></i><br>
            &emsp;&emsp;<code>LogLevel = Finer; Trace</code> - <i>Outputs <code>Finer</code> level for <code>Log4net</code> and <code>Trace</code> level for <code>NLog</code> - since 1.5.0</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>EnterFormat</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>EnterString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i>
        </td>
        <td>
            The format string of log output when entering methods<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>EnterFormat = Enter {0}.{1} ({2}:{3:D})</code> <i>(This is the defalut)</i><br>
            <b>Parameters:</b><br>
            &emsp;&emsp;<code>{0}</code>: The class name<br>
            &emsp;&emsp;<code>{1}</code>: The method name<br>
            &emsp;&emsp;<code>{2}</code>: The file name<br>
            &emsp;&emsp;<code>{3}</code>: The line number
        </td>
    </tr>
    <tr>
        <td>
            <code>LeaveFormat</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>LeaveString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i>
        </td>
        <td>
            The format string of log output when leaving methods<br>
            <b>Example:</b><br>
            <code>LeaveString = Leave {0}.{1} ({2}:{3:D}) duration: {4}</code> <i>(This is the defalut)</i><br>
            <b>Parameters:</b><br>
            &emsp;&emsp;<code>{0}</code>: The class name<br>
            &emsp;&emsp;<code>{1}</code>: The method name<br>
            &emsp;&emsp;<code>{2}</code>: The file name<br>
            &emsp;&emsp;<code>{3}</code>: The line number<br>
            &emsp;&emsp;<code>{4}</code>: The duration since invoking the corresponding </code>Enter</code> method
        </td>
    </tr>
    <tr>
        <td>
            <code>ThreadBoundaryFormat</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>ThreadBoundaryString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i></td>
        <td>
            The format string of log output at threads boundary<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>ThreadBoundaryFormat = ______________________________ Thread {0} ______________________________</code>
            <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The thread ID
        </td>
    </tr>
    <tr>
        <td>
            <code>ClassBoundaryFormat</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>ClassBoundaryString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i></td>
        <td>
            The format string of log output at classes boundary<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>ClassBoundaryFormat = ____ {0} ____</code> <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The class name
        </td>
    </tr>
    <tr>
        <td>
            <code>IndentString</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>CodeIndentString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i>
        </td>
        <td>
            The indentation string for code<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>CodeIndentString = |\s</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> is replaced to a space character</i>
        </td>
    </tr>
    <tr>
        <td><code>DataIndentString</code>
        <td>
            The indentation string for data<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>DataIndentString = \s\s</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> is replaced to a space character</i>
        </td>
    </tr>
    <tr>
        <td><code>LimitString</code>
        <td>
            The string to represent that it has exceeded the limit<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>LimitString = ...</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>NonOutputString</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>NonPrintString</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i>
        </td>
        <td>
            The string to be output instead of not outputting value<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>NonOutputString = ***</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>CyclicReferenceString</code>
        <td>
            The string to represent that the cyclic reference occurs<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>CyclicReferenceString = *** Cyclic Reference ***</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>VarNameValueSeparator</code>
        <td>
            The separator string between the variable name and value<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>VarNameValueSeparator = \s=\s</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> is replaced to a space character</i>
        </td>
    </tr>
    <tr>
        <td><code>KeyValueSeparator</code>
        <td>
            The separator string between the key and value of dictionary
            and between the property/field name and value<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>KeyValueSeparator = :\s</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> is replaced to a space character</i>
        </td>
    </tr>
    <tr>
        <td><code>PrintSuffixFormat</code>
        <td>
            The format string of <code>Print</code> method suffix<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>PrintSuffixFormat = \s({2}:{3:D})</code> <i>(This is the defalut)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> is replaced to a space character</i><br>
            <b>Parameters:</b><br>
            &emsp;&emsp;<code>{0}</code>: The class name<br>
            &emsp;&emsp;<code>{1}</code>: The method name<br>
            &emsp;&emsp;<code>{2}</code>: The file name<br>
            &emsp;&emsp;<code>{3}</code>: The line number
        </td>
    </tr>
    <tr>
        <td>
            <code>CountFormat</code><br>
            &emsp;<i>since 1.5.1</i>
        </td>
        <td>
            The format string of the number of elements of collection<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>CountFormat = \sCount:{0}</code> <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The number of elements<br>
        </td>
    </tr>
    <tr>
        <td>
            <code>MinimumOutputCount</code><br>
            &emsp;<i>since 2.0.0</i>
        </td>
        <td>
            The minimum value to output the number of elements of collection<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>MinimumOutputCount = 5</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>LengthFormat</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <code>StringLengthFormat</code><br>
            &emsp;<i>since 1.5.1</i><br>
            &emsp;<i>Deprecated since 2.0.0</i></td>
        <td>
            The format string of the length of string<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>StringLengthFormat = (Length:{0})</code> <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The string length
        </td>
    </tr>
    <tr>
        <td>
            <code>MinimumOutputLength</code><br>
            &emsp;<i>since 2.0.0</i>
        </td>
        <td>
            The minimum value to output the length of string<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>MinimumOutputLength = 5</code> <i>(This is the defalut)</i><br>
        </td>
    </tr>
    <tr>
        <td><code>DateTimeFormat</code>
        <td>
            The format string of <code>DateTime</code><br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>DateTimeFormat = {0:yyyy-MM-dd HH:mm:ss.fffffffK}</code> <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The <code>DateTime</code> object
        </td>
    </tr>
    <tr>
        <td><code>LogDateTimeFormat</code></td>
        <td>
            The format string of date and time  of the beginning of the line when outputting logs<br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>LogDateTimeFormat = {0:yyyy-MM-dd HH:mm:ss.fff} [{1:D2}] {2}</code> <i>(This is the defalut)</i><br>
            <b>Parameter:</b><br>
            &emsp;&emsp;<code>{0}</code>: The <code>DateTime</code> of log output<br>
            &emsp;&emsp;<code>{1}</code>: The thread ID<br>
            &emsp;&emsp;<code>{2}</code>: The log contents
        </td>
    </tr>
    <tr>
        <td>
            <code>MaximumDataOutputWidth</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <code>MaxDataOutputWidth</code><br>
            &emsp;<i>Deprecated since 2.0.0</i></td>
        <td>
            The maximum output width of data<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>MaximumDataOutputWidth = 70</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>CollectionLimit</code></td>
        <td>
            The limit value of elements for collection to output<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>CollectionLimit = 512</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>StringLimit</code></td>
        <td>
            The limit value of characters for string to output<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>StringLimit = 8192</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>ReflectionNestLimit</code></td>
        <td>
            The The limit value for reflection nesting<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>ReflectionNestLimit = 4</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>NonOutputProperties</code><br>
            &emsp;<i>Renamed since 2.0.0</i><br>
            <br>
            <i><code>NonPrintProperties</code></i><br>
            &emsp;<i>Deprecated since 2.0.0</i>
        </td>
        <td>
            Properties and fields not to be output value<br>
            <b>Example (One value):</b><br>
            &emsp;&emsp;<code>NonOutputProperties = DebugTraceExample.Node.Parent</code><br>
            <b>Example (Multiple values):</b><br>
            &emsp;&emsp;<code>NonOutputProperties = \</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Parent,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Left,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Right</code><br>
            <b>Format:</b><br>
            &emsp;&emsp;<code>&lt;full class name&gt;.&lt;property or field name&gt;</code><br>
            <i>No default value</i>
        </td>
    </tr>
    <tr>
        <td><code>DefaultNameSpace</code>
        <td>
            The default namespace of your C source<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>DefaultNameSpace = DebugTraceExample</code><br>
            <i>No default value</i>
        </td>
    </tr>
    <tr>
        <td><code>DefaultNameSpaceString</code>
        <td>
            The string replacing the default namespace part<br>
            <b>Example:</b><br>
            &emsp;&emsp;<code>DefaultNameSpaceString = \...</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>ReflectionClasses</code>
        <td>
            Classe names that output content by reflection even if <code>ToString</code> method is implemented<br>
            <b>Example (One value):</b><br>
            &emsp;&emsp;<code>ReflectionClasses = DebugTraceExample.Point</code><br>
            <b>Example (Multiple values):</b><br>
            &emsp;&emsp;<code>ReflectionClasses = \</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Point,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Rectangle</code><br>
            <i>No default value</i>
        </td>
    </tr>
    <tr>
        <td><code>OutputNonPublicFields</code>
        <td>
            If <code>true</code>, outputs the contents by reflection even for fields which are not <code>public</code><br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>OutputNonPublicFields = true</code><br>
            &emsp;&emsp;<code>OutputNonPublicFields = false</code> <i>(This is the defalut)</i>
        </td>
    </tr>
    <tr>
        <td><code>OutputNonPublicProperties</code>
        <td>
            If <code>true</code>, outputs the contents by reflection even for properties which are not <code>public</code><br>
            <b>Examples:</b><br>
            &emsp;&emsp;<code>OutputNonPublicProperties = true</code><br>
            &emsp;&emsp;<code>OutputNonPublicProperties = false</code> <i>(This is the defalut)</i>
        </td>
    </tr>
</table>

### 7.1. Adding **DebugTrace.properties** file

You can add the `DebugTrace.properties` file to your projects in the following steps.

1. Select `Add` - `New Item ...` from the context menu of the project.
1. Select `Text File` in the dialog window, set the `Name:` to `DebugTrace.properties` and click `Add` button.
1. Select `Properties` from context menu of the added `DebugTrace.properties`.
1. Change setting of `Copy to Output Directory` in the `Advanced` section of the `Properties` to `Copy if newer` or `Copy always`.

### 7.2. **NonOutputProperties**, **NonOutputString**

DebugTrace use reflection to output object contents if the `ToString` method is not implemented.
If there are other object references, the contents of objects are also output.
However, if there is circular reference, it will automatically detect and suspend output.
You can suppress output by specifying the `NonOutputProperties` property and
can specify multiple values of this property separated by commas.
The value of the property specified by `NonOutputProperties` are output as the string specified by `NonOutputString` (default: `***`).

&emsp;&emsp;_Example of NonOutputProperties_
```properties
NonOutputProperties = DebugTraceExample.Node.Parent
```

&emsp;&emsp;_Example of NonOutputProperties (Multiple specifications)_
```properties
NonOutputProperties = \
    DebugTraceExample.Node.Parent,\
    DebugTraceExample.Node.Left,\
    DebugTraceExample.Node.Right
```

## 8. Using logging libraries

You can output logs using the following libraries besides console output.

&emsp;&emsp;_logging Libraries_
<table>
    <tr><th>Logging Library</th><th>Required package</th></tr>
    <tr><td>log4net</td><td>DebugTrace.Log4net</td></tr>
    <tr><td>NLog</td><td>DebugTrace.NLog</td></tr>
</table>

To use them, add the above package from NuGet.  
The logger name of DebugTrace is `DebugTrace`.

### 8-1. log4net

&emsp;&emsp;_Example of DebugTrace.properties_
```properties
# DebugTrace.properties
Logger = Log4net
```

&emsp;&emsp;_Additional example of AssemblyInfo.cs_
```c#
[assembly: log4net.Config.XmlConfigurator(ConfigFile=@"Log4net.config", Watch=true)]
```

&emsp;&emsp;_Example of Log4net.config_
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <log4net>
    <appender name="A" type="log4net.Appender.FileAppender">
      <File value="/Logs/DebugTrace/Log4net.log" />
      <AppendToFile value="true" />
      <ImmediateFlush value="true" />
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <layout type="log4net.Layout.PatternLayout">
        <ConversionPattern value="%date [%thread] %-5level %logger %message%n" />
      </layout>
    </appender>
    <root>
      <level value="DEBUG" />
      <appender-ref ref="A" />
    </root>
  </log4net>
</configuration>
```

### 8-2. NLog

&emsp;&emsp;_Example of DebugTrace.properties_
```log
# DebugTrace.properties
Logger = NLog
```

&emsp;&emsp;_Example of NLog.config_
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="/Logs/DebugTrace/NLog-internal.log">
  <targets>
    <target xsi:type="File" name="f" fileName="/Logs/DebugTrace/NLog.log" encoding="utf-8"
            layout="${longdate} [${threadid}] ${uppercase:${level}} ${logger} ${message}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Debug" writeTo="f" />
  </rules>
</nlog>
```

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
