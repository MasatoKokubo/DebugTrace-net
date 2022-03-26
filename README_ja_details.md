以下は、DebugTrace-netを使用したC#, Visual Basicの例とそれを実行した際のログです。

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

&emsp;&emsp;_コンソール出力 / C#_
```log
2022-05-22 21:39:45.227 [04] DebugTrace-net 3.0.0 on .NET 6.0.5
2022-05-22 21:39:45.231 [04]   properties file path: Z:\Develop\C#\DebugTrace\DebugTrace\DebugTraceTest\bin\Debug\net6.0\DebugTrace.properties
2022-05-22 21:39:45.231 [04]   logger: DebugTrace.Console+Error
2022-05-22 21:39:45.232 [04] 
2022-05-22 21:39:45.232 [04] ______________________________ Thread 4 ______________________________
2022-05-22 21:39:45.232 [04] 
2022-05-22 21:39:45.265 [04] Enter Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:17)
2022-05-22 21:39:45.275 [04] | 
2022-05-22 21:39:45.275 [04] | contacts = Readme.Contact[2] {
2022-05-22 21:39:45.275 [04] |   Readme.Contact {
2022-05-22 21:39:45.275 [04] |     ____ Readme.Entity ____
2022-05-22 21:39:45.275 [04] |     ID: 1
2022-05-22 21:39:45.275 [04] |     ____ Readme.ContactBase ____
2022-05-22 21:39:45.275 [04] |     FirstName: (Length:5)"Akane", LastName: (Length:5)"Apple"
2022-05-22 21:39:45.275 [04] |     ____ Readme.Contact ____
2022-05-22 21:39:45.275 [04] |     Birthday: 1991-02-03 00:00:00.0000000
2022-05-22 21:39:45.275 [04] |   },
2022-05-22 21:39:45.275 [04] |   Readme.Contact {
2022-05-22 21:39:45.275 [04] |     ____ Readme.Entity ____
2022-05-22 21:39:45.275 [04] |     ID: 2
2022-05-22 21:39:45.275 [04] |     ____ Readme.ContactBase ____
2022-05-22 21:39:45.275 [04] |     FirstName: (Length:6)"Yukari", LastName: (Length:5)"Apple"
2022-05-22 21:39:45.275 [04] |     ____ Readme.Contact ____
2022-05-22 21:39:45.275 [04] |     Birthday: 1992-03-04 00:00:00.0000000
2022-05-22 21:39:45.275 [04] |   }
2022-05-22 21:39:45.275 [04] | } (ReadmeExamples.cs:23)
2022-05-22 21:39:45.275 [04] | 
2022-05-22 21:39:45.276 [04] Leave Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:25) duration: 00:00:00.0102507
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

&emsp;&emsp;_コンソール出力 / Visual Basic_
```log
2022-05-22 21:29:05.534 [05] DebugTrace-net 2.1.0 on .NET Core 3.1.25
2022-05-22 21:29:05.535 [05]   properties file path: Z:\Develop\C#\DebugTrace\DebugTrace\DebugTraceVBTest\bin\Debug\netcoreapp3.1\DebugTrace.properties
2022-05-22 21:29:05.535 [05]   logger: DebugTrace.Console+Error
2022-05-22 21:29:05.536 [05] 
2022-05-22 21:29:05.536 [05] ______________________________ Thread 5 ______________________________
2022-05-22 21:29:05.536 [05] 
2022-05-22 21:29:05.567 [05] Enter DebugTraceVBTest.Readme.ReadmeExample1.Example1() (ReadmeExamples.vb:12)
2022-05-22 21:29:05.576 [05] | 
2022-05-22 21:29:05.576 [05] | contacts = DebugTraceVBTest.Readme.Contact(Length: 2) {
2022-05-22 21:29:05.576 [05] |   DebugTraceVBTest.Readme.Contact {
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.Entity ____
2022-05-22 21:29:05.576 [05] |     Id: 1
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.ContactBase ____
2022-05-22 21:29:05.576 [05] |     FirstName: (Length:5)"Akane", LastName: (Length:5)"Apple"
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.Contact ____
2022-05-22 21:29:05.576 [05] |     Birthday: 1991-02-03 00:00:00.0000000
2022-05-22 21:29:05.576 [05] |   },
2022-05-22 21:29:05.576 [05] |   DebugTraceVBTest.Readme.Contact {
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.Entity ____
2022-05-22 21:29:05.576 [05] |     Id: 2
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.ContactBase ____
2022-05-22 21:29:05.576 [05] |     FirstName: (Length:6)"Yukari", LastName: (Length:5)"Apple"
2022-05-22 21:29:05.576 [05] |     ____ DebugTraceVBTest.Readme.Contact ____
2022-05-22 21:29:05.576 [05] |     Birthday: 1992-03-04 00:00:00.0000000
2022-05-22 21:29:05.576 [05] |   }
2022-05-22 21:29:05.576 [05] | } (ReadmeExamples.vb:18)
2022-05-22 21:29:05.576 [05] | 
2022-05-22 21:29:05.577 [05] Leave DebugTraceVBTest.Readme.ReadmeExample1.Example1() (ReadmeExamples.vb:20) duration: 00:00:00.0091880
```

### 3.1 using System.Diagnostics(Imports System.Diagnostics)を行っている場合

`System.Diagnostics` 名前空間を `using`(`Imports`) している場合は、`DebugTrace.CSharp.Trace` (`DebugTrace.VisualBaisc.Trace`) プロパティと `System.Diagnostics.Trace` クラスが重なるため、`Trace` プロパティの代わりに `Trace_` プロパティを使用してください。

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
Imports DebugTrace.VisualBasic ' for Debugging

Namespace Global.Readme
    Public Class ReadmeExample
        Public Shared Sub Main(args As String())
            Trace_.Enter() ' for Debugging
```

## 4. インタフェース、クラス

主に以下のインタフェース、クラスがあります。

&emsp;&emsp;_インタフェース、クラス_
<table>
    <tr><th>名 前</th><th>スーパークラス、実装するインタフェース</th><th>説 明</th></tr>
    <tr><td>DebugTrace.ITrace       </td><td>なし              </td><td>Trace処理のインタフェース</td></tr>
    <tr><td>DebugTrace.TraceBase    </td><td>DebugTrace.ITrace </td><td>Trace処理のベースクラス</td></tr>
    <tr><td>DebugTrace.CSharp       </td><td>DebugTrace.Trace  </td><td>C#用のTrace処理のクラス</td></tr>
    <tr><td>DebugTrace.VisualBasic  </td><td>DebugTrace.Trace  </td><td>VisualBasic用のTrace処理のクラス</td></tr>
    <tr><td>DebugTrace.ILogger      </td><td>なし              </td><td>ログ出力のインタフェース</td></tr>
    <tr><td>DebugTrace.Console      </td><td>DebugTrace.ILogger</td><td><code>DebugTrace.Console.Out</code>と<code>DebugTrace.Console.Error</code>の抽象スーパークラス</td></tr>
    <tr><td>DebugTrace.Console.Out  </td><td>DebugTrace.Console</td><td>ログを標準出力に出力するクラス</td></tr>
    <tr><td>DebugTrace.Console.Error</td><td>DebugTrace.Console</td><td>ログを標準エラー出力に出力するクラス</td></tr>
    <tr><td>DebugTrace.Diagnostics<br>&emsp;<i>1.6.0 より</i></td><td>DebugTrace.ILogger</td><td><code>DebugTrace.Diagnostics.Debug</code>と<code>DebugTrace.Diagnostics.Trace</code>の抽象スーパークラス</td></tr>
    <tr><td>DebugTrace.Diagnostics.Debug<br>&emsp;<i>1.6.0 より</i></td><td>DebugTrace.Diagnostics</td><td>ログを<code>System.Diagnostics.Debug</code>を使用して出力するクラス</td></tr>
    <tr><td>DebugTrace.Diagnostics.Trace<br>&emsp;<i>1.6.0 より</i></td><td>DebugTrace.Diagnostics</td><td>ログを<code>System.Diagnostics.Trace</code>を使用して出力するクラス</td></tr>
</table>

## 5. DebugTrace.CSharpおよびDebugTrace.VisualBasicクラスのプロパティ

`DebugTrace.CSharp` クラスと `DebugTrace.VisualBasic` クラスには自身の型のインスタンスとして `Trace` および `Trace_` プロパティがあります。

## 6. ITraceインタフェースのプロパティおよびメソッド

以下のプロパティおよびメソッドがあります。

&emsp;&emsp;_プロパティ表_
<table>
    <tr><th>名 前</th><th>説 明</th></tr>
    <tr>
        <td>IsEnabled</td>
        <td>ログ出力が有効なら<code>true</code>、そうでなければ<code>false</code> (<code>get</code>のみ)</td>
    </tr>
    <tr>
        <td>LastLog</td>
        <td>最後に出力したログ文字列 (<code>get</code>のみ</td>
    </tr>
</table>

&emsp;&emsp;_メソッド表_
<table>
    <tr><th>名 前</th><th>DebugTrace<br>バージョン</th><th>引 数</th><th>戻り値</th><th>説 明</th></tr>
    <tr>
        <td><code>ResetNest</code></td>
        <td></td>
        <td><i>なし</i></td>
        <td><i>なし</i></td>
        <td>現在のスレッドのネストレベルを初期化する</td>
    </tr>
    <tr>
        <td><code>Enter</code></td>
        <td></td>
        <td><i>なし</i></td>
        <td><code>int</code> スレッドID</td>
        <td>メソッドの開始をログに出力する</td>
    </tr>
    <tr>
        <td><code>Leave</code></td>
        <td></td>
        <td><code>int threadId</code>: スレッドID<br>(デフォルト: <code>-1</code>)</td>
        <td><i>なし</i></td>
        <td>メソッドの終了をログに出力する</td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 以降</td>
        <td rowspan=2><code>string message</code>: メッセージ</td>
        <td>メッセージ</td>
        <td rowspan=2>メッセージをログに出力する</td>
    </tr>
    <tr>
        <td>2.0.3 以前</td>
        <td><i>なし</i></td>
    </tr>
    <tr>
        <td rowspan=2><code>Print</code></td>
        <td>2.1.0 以降</td>
        <td rowspan=2><code>Func&lt;string&gt; messageSupplier</code>: メッセージを返す関数</td>
        <td><code>messageSupplier</code> から取得したメッセージ</td>
        <td rowspan=2><code>messageSupplier</code> からメッセージを取得してログに出力する</td>
    </tr>
    <tr>
        <td>2.0.3 以前</td>
        <td><i>なし</i></td>
    </tr>
    <tr>
        <td rowspan=3><code>Print</code></td>
        <td>3.0.0 以降</td>
        <td>
            <code>string name</code>: 値の名前<br>
            <code>T value</code>: 値
        </td>
        <td rowspan=2>値</td>
        <td rowspan=3><code>name = value</code> の形式でログに出力する</td>
    </tr>
    <tr>
        <td>2.1.0 以降</td>
        <td rowspan=2>
            <code>string name</code>: 値の名前<br>
            <code>object value</code>: 値
        </td>
    </tr>
    <tr>
        <td>2.0.3 以前</td>
        <td><i>なし</i></td>
    </tr>
    <tr>
        <td rowspan=3><code>Print</code></td>
        <td>3.0.0 以降</td>
        <td>
            <code>string name</code>: 値の名前<br>
            <code>Func&lt;T&gt; valueSupplier</code>: 値を返す関数
        </td>
        <td rowspan=2>値 obtained from <code>valueSupplier</code></td>
        <td>
            <code>valueSupplier</code> から値を取得して <code>name = value</code> の形式でログに出力する
        </td>
    </tr>
    <tr>
        <td>2.1.0 以降</td>
        <td rowspan=2>
            <code>string name</code>: 値の名前<br>
            <code>Func&lt;object&gt; valueSupplier</code>: 値を返す関数
        </td>
    </tr>
    <tr>
        <td>2.0.3 以前</td>
        <td><i>なし</i></td>
    </tr>
    <tr>
        <td><code>PrintStack</code></td>
        <td>1.6.0 より</td>
        <td><code>int maxCount</code>: 出力するスタック要素の最大数</td>
        <td><i>なし</i></td>
        <td>コールスタックをログに出力する</td>
    </tr>
</table>


## 7. **DebugTrace.properties** ファイルのプロパティ

DebugTrace は、カレントディレクトリにある `DebugTrace.properties` ファイルを起動時に読み込みます。
`DebugTrace.properties` ファイルでは以下のプロパティを指定できます。

&emsp;&emsp;_Table of DebugTrace.properties_
<table>
    <tr><th>Property Name</th><th>Description</th></tr>
    <tr>
        <td><code>Logger</code></td>
        <td>
            DebugTrace が使用するロガー<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>Logger = Console+Out</code> - <i>コンソール(標準出力)へ出力</i><br>
            &emsp;&emsp;<code>Logger = Console+Error</code> - <i>コンソール(標準エラー出力)へ出力 <i>(デフォルト)</i></i><br>
            &emsp;&emsp;<code>Logger = Diagnostics+Debug</code> - <i><code>System.Diagnostics.Debug</code> クラスを使用して出力 - 1.6.0 より</i><br>
            &emsp;&emsp;<code>Logger = Diagnostics+Trace</code> - <i><code>System.Diagnostics.Trace</code> クラスを使用して出力 - 1.6.0 より</i><br>
            &emsp;&emsp;<code>Logger = Log4net</code> - <i><code>Log4net</code> を使用して出力</i><br>
            &emsp;&emsp;<code>Logger = NLog</code> - <i><code>NLog</code> を使用して出力</i><br>
            <b>設定例(複数):</b> - <i>1.5.0 より</i><br>
            &emsp;&emsp;<code>Logger = Console+Out; Log4net</code> - <i>Outputs to the console (stdout) and using <code>Log4net</code></i>
        </td>
    </tr>
    <tr>
        <td><code>LogLevel</code></td>
        <td>
            出力する際に使用するログレベル<br>
            <b>Log4net を使用する際の設定例:</b><br>
            &emsp;&emsp;<code>LogLevel = All</code><br>
            &emsp;&emsp;<code>LogLevel = Finest</code><br>
            &emsp;&emsp;<code>LogLevel = Verbose</code><br>
            &emsp;&emsp;<code>LogLevel = Finer</code><br>
            &emsp;&emsp;<code>LogLevel = Trace</code><br>
            &emsp;&emsp;<code>LogLevel = Fine</code><br>
            &emsp;&emsp;<code>LogLevel = Debug</code> <i>(デフォルト)</i><br>
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
            <b>NLog を使用する際の設定例:</b><br>
            &emsp;&emsp;<code>LogLevel = Trace</code><br>
            &emsp;&emsp;<code>LogLevel = Debug</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;<code>LogLevel = Info</code><br>
            &emsp;&emsp;<code>LogLevel = Warn</code><br>
            &emsp;&emsp;<code>LogLevel = Error</code><br>
            &emsp;&emsp;<code>LogLevel = Fatal</code><br>
            &emsp;&emsp;<code>LogLevel = Off</code><br>
            <b>Log4net と NLog を同時使用する際の設定例:</b> (Logger = Log4net; NLog)<br>
            &emsp;&emsp;<code>LogLevel = Debug</code> - <i><code>Log4net</code> と <code>NLog</code> の両方に <code>Debug</code> レベルで出力</code></i><br>
            &emsp;&emsp;<code>LogLevel = Finer; Trace</code> - <i><code>Log4net</code> では <code>Finer </code>レベル、<code>NLog</code> では <code>Trace</code> レベルで出力 - 1.5.0 より</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>EnterFormat</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>EnterString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i>
        </td>
        <td>
            メソッドに入る際に出力するログのフォーマット文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>EnterFormat = Enter {0}.{1} ({2}:{3:D})</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: クラス名<br>
            &emsp;&emsp;<code>{1}</code>: メソッド名<br>
            &emsp;&emsp;<code>{2}</code>: ファイル名<br>
            &emsp;&emsp;<code>{3}</code>: 行番号
        </td>
    </tr>
    <tr>
        <td>
            <code>LeaveFormat</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>LeaveString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i>
        </td>
        <td>
            メソッドから出る際のログ出力のフォーマット文字列<br>
            <b>設定例:</b><br>
            <code>LeaveString = Leave {0}.{1} ({2}:{3:D}) duration: {4}</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: クラス名<br>
            &emsp;&emsp;<code>{1}</code>: メソッド名<br>
            &emsp;&emsp;<code>{2}</code>: ファイル名<br>
            &emsp;&emsp;<code>{3}</code>: 行番号<br>
            &emsp;&emsp;<code>{4}</code>: 対応する <code>Enter</code> メソッドを呼び出してからの時間
        </td>
    </tr>
    <tr>
        <td>
            <code>ThreadBoundaryFormat</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>ThreadBoundaryString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i></td>
        <td>
            スレッド境界のログ出力の文字列フォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>ThreadBoundaryFormat = ______________________________ Thread {0} ______________________________</code>
            <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: スレッドID
        </td>
    </tr>
    <tr>
        <td>
            <code>ClassBoundaryFormat</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>ClassBoundaryString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i></td>
        <td>
            クラス境界のログ出力の文字列フォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>ClassBoundaryFormat = ____ {0} ____</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: クラス名
        </td>
    </tr>
    <tr>
        <td>
            <code>IndentString</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>CodeIndentString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i>
        </td>
        <td>
            コードのインデント文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>CodeIndentString = |\s</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> は空白文字に置き換えられる</i>
        </td>
    </tr>
    <tr>
        <td><code>DataIndentString</code>
        <td>
            データのインデント 文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>DataIndentString = \s\s</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> は空白文字に置き換えられる</i>
        </td>
    </tr>
    <tr>
        <td><code>LimitString</code>
        <td>
            制限を超えた場合に出力する文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>LimitString = ...</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>NonOutputString</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>NonPrintString</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i>
        </td>
        <td>
            値を出力しない場合に代わりに出力する文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>NonOutputString = ***</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>CyclicReferenceString</code>
        <td>
            T循環参照している場合に出力する文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>CyclicReferenceString = *** Cyclic Reference ***</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>VarNameValueSeparator</code>
        <td>
            変数名と値のセパレータ文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>VarNameValueSeparator = \s=\s</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> は空白文字に置き換えられる</i>
        </td>
    </tr>
    <tr>
        <td><code>KeyValueSeparator</code>
        <td>
            辞書のキーと値およびプロパティ/フィールド名と値のセパレータ文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>KeyValueSeparator = :\s</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> は空白文字に置き換えられる</i>
        </td>
    </tr>
    <tr>
        <td><code>PrintSuffixFormat</code>
        <td>
            <code>Print</code> メソッドで付加される文字列のフォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>PrintSuffixFormat = \s({2}:{3:D})</code> <i>(デフォルト)</i><br>
            &emsp;&emsp;&emsp;<i><code>\s</code> は空白文字に置き換えられる</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: クラス名<br>
            &emsp;&emsp;<code>{1}</code>: メソッド名<br>
            &emsp;&emsp;<code>{2}</code>: ファイル名<br>
            &emsp;&emsp;<code>{3}</code>: 行番号
        </td>
    </tr>
    <tr>
        <td>
            <code>CountFormat</code><br>
            &emsp;<i>1.5.1 より</i>
        </td>
        <td>
            コレクションの要素数のフォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>CountFormat = \sCount:{0}</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: The number of elements<br>
        </td>
    </tr>
    <tr>
        <td>
            <code>MinimumOutputCount</code><br>
            &emsp;<i>since 2.0.0</i>
        </td>
        <td>
            コレクションの要素数を出力する最小値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>MinimumOutputCount = 5</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>LengthFormat</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <code>StringLengthFormat</code><br>
            &emsp;<i>1.5.1 より</i><br>
            &emsp;<i>2.0.0 より非推奨</i></td>
        <td>
            文字列長のフォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>StringLengthFormat = (Length:{0})</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: 文字列長
        </td>
    </tr>
    <tr>
        <td>
            <code>MinimumOutputLength</code><br>
            &emsp;<i>since 2.0.0</i>
        </td>
        <td>
            文字列長を出力する最小値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>MinimumOutputLength = 5</code> <i>(デフォルト)</i><br>
        </td>
    </tr>
    <tr>
        <td><code>DateTimeFormat</code>
        <td>
            日時のフォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>DateTimeFormat = {0:yyyy-MM-dd HH:mm:ss.fffffffK}</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: The </code>DateTime</code> object
        </td>
    </tr>
    <tr>
        <td><code>LogDateTimeFormat</code></td>
        <td>
            ログの行頭の日時のフォーマット<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>LogDateTimeFormat = {0:yyyy-MM-dd HH:mm:ss.fff} [{1:D2}] {2}</code> <i>(デフォルト)</i><br>
            <b>パラメータ:</b><br>
            &emsp;&emsp;<code>{0}</code>: ログの出力日時<br>
            &emsp;&emsp;<code>{1}</code>: スレッドID<br>
            &emsp;&emsp;<code>{2}</code>: ログ内容
        </td>
    </tr>
    <tr>
        <td>
            <code>MaximumDataOutputWidth</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <code>MaxDataOutputWidth</code><br>
            &emsp;<i>2.0.0 より非推奨</i></td>
        <td>
            データの出力幅の最大値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>MaximumDataOutputWidth = 70</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>CollectionLimit</code></td>
        <td>
            コレクションの要素の出力数の制限値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>CollectionLimit = 512</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>StringLimit</code></td>
        <td>
            文字列の出力文字数の制限値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>StringLimit = 8192</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>ReflectionNestLimit</code></td>
        <td>
            リフレクションのネスト数の制限値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>ReflectionNestLimit = 4</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>NonOutputProperties</code><br>
            &emsp;<i>2.0.0 より名称変更</i><br>
            <br>
            <i><code>NonPrintProperties</code></i><br>
            &emsp;<i>2.0.0 より非推奨</i>
        </td>
        <td>
            出力しないプロパティ名およびフィールド名の配列<br>
            <b>設定例(1つ):</b><br>
            &emsp;&emsp;<code>NonOutputProperties = DebugTraceExample.Node.Parent</code><br>
            <b>設定例(複数):</b><br>
            &emsp;&emsp;<code>NonOutputProperties = \</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Parent,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Left,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Node.Right</code><br>
            <b>値のフォーマット:</b><br>
            &emsp;&emsp;<code>&lt;フルクラス名&gt;.&lt;プロパティ名またはフィールド名&gt;</code><br>
            <i>デフォルトはなし</i>
        </td>
    </tr>
    <tr>
        <td><code>DefaultNameSpace</code>
        <td>
            デフォルトの名前空間<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>DefaultNameSpace = DebugTraceExample</code><br>
            <i>デフォルトはなし</i>
        </td>
    </tr>
    <tr>
        <td><code>DefaultNameSpaceString</code>
        <td>
            デフォルトの名前空間を置き換える文字列<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>DefaultNameSpaceString = \...</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>ReflectionClasses</code>
        <td>
            <code>ToString</code> メソッドを実装していてもリフレクションで内容を出力するクラス名のセット<br>
            <b>設定例(1つ):</b><br>
            &emsp;&emsp;<code>ReflectionClasses = DebugTraceExample.Point</code><br>
            <b>設定例(複数):</b><br>
            &emsp;&emsp;<code>ReflectionClasses = \</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Point,\</code><br>
            &emsp;&emsp;&emsp;&emsp;<code>DebugTraceExample.Rectangle</code><br>
            <i>デフォルトはなし</i>
        </td>
    </tr>
    <tr>
        <td><code>OutputNonPublicFields</code>
        <td>
            <code>true</code> の場合、<code>public</code> ではないフィールドもリフレクションで内容を出力する<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>OutputNonPublicFields = true</code><br>
            &emsp;&emsp;<code>OutputNonPublicFields = false</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td><code>OutputNonPublicProperties</code>
        <td>
            <code>true</code> の場合、<code>public</code> ではないプロパティもリフレクションで内容を出力する<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>OutputNonPublicProperties = true</code><br>
            &emsp;&emsp;<code>OutputNonPublicProperties = false</code> <i>(デフォルト)</i>
        </td>
    </tr>
</table>

### 7.1. DebugTrace.properties ファイルの追加

以下の手順でプロジェクトに `DebugTrace.properties` ファイルを追加します。

1. プロジェクトのコンテキストメニューから `追加` - `新しい項目...` を選択する。
1. ダイアログで、 `テキスト ファイル` を選択し、名前を `DebugTrace.properties` にして `追加` ボタンをクリックする。
1. 追加された `DebugTrace.properties` のコンテキストメニューから `プロパティ` を選択する。
1. `プロパティ` の `詳細` セクションにある `出力ディレクトリにコピー` の設定値を `新しい場合はコピーする` または `常にコピーする` に変更する。


### 7.2. **NonOutputProperties**, **NonOutputString**

DebugTrace use reflection to output object contenDebugTrace は、 `ToString` メソッドが実装されていない場合は、リフレクションを使用してオブジェクト内容を出力します。
他のオブジェクトの参照があれば、そのオブジェクトの内容も出力します。
ただし循環参照がある場合は、自動的に検出して出力を中断します。  
`NonOutputProperties` プロパティを指定して出力を抑制する事もできます。
このプロパティの値は、カンマ区切りで複数指定できます。  
`NonOutputProperties` で指定されたプロパティの値は、 `NonOutputString` で指定された文字列 (デフォルト: `***`) で出力されます。

&emsp;&emsp;_NonOutputProperties の例_
```properties
NonOutputProperties = DebugTraceExample.Node.Parent
```

&emsp;&emsp;_NonOutputProperties の例 (複数指定)_
```properties
NonOutputProperties = \
    DebugTraceExample.Node.Parent,\
    DebugTraceExample.Node.Left,\
    DebugTraceExample.Node.Right
```

## 8. ロギングライブラリの使用

You can output logs using the following libraries besides console output.

&emsp;&emsp;_logging Libraries_
<table>
    <tr><th>ロギングライブラリ</th><th>必要なパッケージ</th></tr>
    <tr><td>log4net</td><td>DebugTrace.Log4net</td></tr>
    <tr><td>NLog</td><td>DebugTrace.NLog</td></tr>
</table>

使用する場合は、上記パッケージを NuGet から追加してください。  
ロギングライブラリを使用する際の DebugTrace のロガー名は、`DebugTrace` です。

### 8-1. log4net

&emsp;&emsp;_DebugTrace.properties の例_
```properties
# DebugTrace.properties
Logger = Log4net
```

&emsp;&emsp;_AssemblyInfo.cs の追加例_
```c#
[assembly: log4net.Config.XmlConfigurator(ConfigFile=@"Log4net.config", Watch=true)]
```

&emsp;&emsp;_Log4net.config の例_
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <log4net>
    <appender name="A" type="log4net.Appender.FileAppender">
      <File value="C:/Logs/DebugTrace/Log4net.log" />
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

&emsp;&emsp;_DebugTrace.properties の例_
```log
# DebugTrace.properties
Logger = NLog
```

&emsp;&emsp;_NLog.config の例_
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="C:/Logs/DebugTrace/NLog-internal.log">
  <targets>
    <target xsi:type="File" name="f" fileName="C:/Logs/DebugTrace/NLog.log" encoding="utf-8"
            layout="${longdate} [${threadid}] ${uppercase:${level}} ${logger} ${message}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Debug" writeTo="f" />
  </rules>
</nlog>
```

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
