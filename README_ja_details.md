以下は、DebugTrace-netを使用したC#の例とそれを実行した際のログです。

&emsp;&emsp;_ReadmeExample.cs_
```c#
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace; // TODO: Debug

namespace Readme;

/// <summary>ReadmeExample1</summary>
[TestClass]
public class ReadmeExample1 {
    /// <summary>Example1</summary>
    [TestMethod]
    public void Example1() {
        Trace.Enter(); // TODO: Debug

        var contacts = new [] {
            new Contact(1, "Akane" , "Apple", new DateTime(1991, 2, 3)),
            new Contact(2, "Yukari", "Apple", new DateTime(1992, 3, 4))
        };
        Trace.Print("contacts", contacts); // TODO: Debug

        Trace.Leave(); // TODO: Debug
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
2024-06-29 12:00:40.799 [04] DebugTrace-net 3.0.0 on .NET 6.0.31
2024-06-29 12:00:40.802 [04]   properties file path: Z:\Develop\DebugTrace\DebugTrace-net\DebugTraceTest\bin\Debug\net6.0\DebugTrace.properties
2024-06-29 12:00:40.802 [04]   logger: DebugTrace.Console+Error
2024-06-29 12:00:40.803 [04] 
2024-06-29 12:00:40.803 [04] ______________________________ Thread 4 ______________________________
2024-06-29 12:00:40.803 [04] 
2024-06-29 12:00:40.845 [04] Enter Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:17)
2024-06-29 12:00:40.855 [04] | 
2024-06-29 12:00:40.855 [04] | contacts = Readme.Contact[2] {
2024-06-29 12:00:40.855 [04] |   Readme.Contact {
2024-06-29 12:00:40.855 [04] |     ____ Readme.Entity ____
2024-06-29 12:00:40.855 [04] |     ID: 1
2024-06-29 12:00:40.855 [04] |     ____ Readme.ContactBase ____
2024-06-29 12:00:40.855 [04] |     FirstName: "Akane", LastName: "Apple"
2024-06-29 12:00:40.855 [04] |     ____ Readme.Contact ____
2024-06-29 12:00:40.855 [04] |     Birthday: 1991-02-03 00:00:00.0000000
2024-06-29 12:00:40.855 [04] |   },
2024-06-29 12:00:40.855 [04] |   Readme.Contact {
2024-06-29 12:00:40.855 [04] |     ____ Readme.Entity ____
2024-06-29 12:00:40.855 [04] |     ID: 2
2024-06-29 12:00:40.855 [04] |     ____ Readme.ContactBase ____
2024-06-29 12:00:40.855 [04] |     FirstName: "Yukari", LastName: "Apple"
2024-06-29 12:00:40.855 [04] |     ____ Readme.Contact ____
2024-06-29 12:00:40.855 [04] |     Birthday: 1992-03-04 00:00:00.0000000
2024-06-29 12:00:40.855 [04] |   }
2024-06-29 12:00:40.855 [04] | } (ReadmeExamples.cs:23)
2024-06-29 12:00:40.855 [04] | 
2024-06-29 12:00:40.856 [04] Leave Readme.ReadmeExample1.Example1() (ReadmeExamples.cs:25) duration: 00:00:00.0102235
```

### 3.1 `using System.Diagnostics`を行っている場合

`System.Diagnostics`名前空間を`using`している場合は、`DebugTrace.Trace`クラスと`System.Diagnostics.Trace`クラスが重なるため、名前空間付きの`DebugTrace.Trace`を使用してください。

&emsp;&emsp;_ReadmeExample.cs_
```c#
using System.Diagnostics;

namespace Readme {
    public class ReadmeExample {
        public static void Main(string[] args) {
            DebugTrace.Trace.Enter(); // TODO: Debug
```

## 4. インタフェース、クラス

主に以下のインタフェース、クラスがあります。

&emsp;&emsp;_インタフェース、クラス_
<table>
    <tr><th>名 前</th><th>スーパークラス、実装するインタフェース</th><th>説 明</th></tr>
    <tr><td>DebugTrace.Trace            </td><td>なし                   </td><td>トレース処理のクラス</td></tr>
    <tr><td>DebugTrace.ILogger          </td><td>なし                   </td><td>ログ出力のインタフェース</td></tr>
    <tr><td>DebugTrace.Console          </td><td>DebugTrace.ILogger    </td><td><code>DebugTrace.Console.Out</code>と<code>DebugTrace.Console.Error</code>の抽象スーパークラス</td></tr>
    <tr><td>DebugTrace.Console.Out      </td><td>DebugTrace.Console    </td><td>ログを標準出力に出力するクラス</td></tr>
    <tr><td>DebugTrace.Console.Error    </td><td>DebugTrace.Console    </td><td>ログを標準エラー出力に出力するクラス</td></tr>
    <tr><td>DebugTrace.Diagnostics      </td><td>DebugTrace.ILogger    </td><td><code>DebugTrace.Diagnostics.Debug</code>と<code>DebugTrace.Diagnostics.Trace</code>の抽象スーパークラス</td></tr>
    <tr><td>DebugTrace.Diagnostics.Debug</td><td>DebugTrace.Diagnostics</td><td>ログを<code>System.Diagnostics.Debug</code>を使用して出力するクラス</td></tr>
    <tr><td>DebugTrace.Diagnostics.Trace</td><td>DebugTrace.Diagnostics</td><td>ログを<code>System.Diagnostics.Trace</code>を使用して出力するクラス</td></tr>
</table>

## 5. Traceクラスのプロパティおよびメソッド

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
    <tr><th>名 前</th><th>引 数</th><th>戻り値</th><th>説 明</th></tr>
    <tr>
        <td><code>ResetNest</code></td>
        <td><i>なし</i></td>
        <td><i>なし</i></td>
        <td>現在のスレッドのネストレベルを初期化する</td>
    </tr>
    <tr>
        <td><code>Enter</code></td>
        <td><i>なし</i></td>
        <td><code>int</code> スレッドID</td>
        <td>メソッドの開始をログに出力する</td>
    </tr>
    <tr>
        <td><code>Leave</code></td>
        <td><code>int threadId</code>: スレッドID<br>(デフォルト: <code>-1</code>)</td>
        <td><i>なし</i></td>
        <td>メソッドの終了をログに出力する</td>
    </tr>
    <tr>
        <td><code>Print</code></td>
        <td><code>string message</code>: メッセージ</td>
        <td>メッセージ</td>
        <td>メッセージをログに出力する</td>
    </tr>
    <tr>
        <td><code>Print</code></td>
        <td><code>Func&lt;string&gt; messageSupplier</code>: メッセージを返す関数</td>
        <td><code>messageSupplier</code> から取得したメッセージ</td>
        <td><code>messageSupplier</code> からメッセージを取得してログに出力する</td>
    </tr>
    <tr>
        <td><code>Print</code></td>
        <td>
            <li><code>string name</code>: 値の名前<br>
            <li><code>T value</code>: 値<br>
            <li><code>bool forceReflection = false</code>:<br>
                <small>trueの場合、ToString()メソッドがあってもリフレクションを使用して出力</small><br>
            <br>
            <small>以下はデフォルト値ではない場合は、DebugTrace.propertiesの設定を上書きする。</small><br>
            <br>
            <li><code>bool? outputNonPublicFields = null</code>:<br>
                <small>true の場合、リフレクション使用時に非公開フィールドを出力する</small><br>
            <li><code>bool? outputNonPublicProperties = null</code>:<br>
                <small>true の場合、リフレクション使用時に非公開プロパティを出力する</small><br>
            <li><code>int minimumOutputCount = -1</code>:<br>
                <small>IDictionaryおよびIEnumerableの要素数を出力するための最小値</small><br>
            <li><code>int minimumOutputLength = -1</code>:<br>
                <small>文字列の長さを出力するための最小値</small><br>
            <li><code>int collectionLimit = -1</code>:<br>
                <small>IDictionaryおよびIEnumerableの出力する要素数の制限値</small><br>
            <li><code>int stringLimit = -1</code>:<br>
                <small>文字列の出力する文字数の制限値</small><br>
            <li><code>reflectionNestLimit = -1</code>:<br>
                <small>リフレクション使用時のネスト制限値</small>
        </td>
        <td>値</td>
        <td><code>name = value</code> の形式でログに出力する</td>
    </tr>
    <tr>
        <td><code>Print</code></td>
        <td>
            <li><code>string name</code>: 値の名前<br>
            <li><code>Func&lt;T&gt; valueSupplier</code>: 値を返す関数<br>
            <br>
            <small>以下は上を参照</small><br>
            <li><code>bool forceReflection = false</code><br>
            <li><code>bool? outputNonPublicFields = null</code><br>
            <li><code>bool? outputNonPublicProperties = null</code><br>
            <li><code>int minimumOutputCount = -1</code><br>
            <li><code>int minimumOutputLength = -1</code><br>
            <li><code>int collectionLimit = -1</code><br>
            <li><code>int stringLimit = -1</code><br>
            <li><code>reflectionNestLimit = -1</code><br>
        </td>
        <td><code>valueSupplier</code>から取得した値</td>
        <td>
            <code>valueSupplier</code> から値を取得して <code>name = value</code> の形式でログに出力する
        </td>
    </tr>
    <tr>
        <td><code>PrintStack</code></td>
        <td><code>int maxCount</code>: 出力するスタック要素の最大数</td>
        <td><i>なし</i></td>
        <td>コールスタックをログに出力する</td>
    </tr>
</table>


## 6. **DebugTrace.properties** ファイルのプロパティ

DebugTrace は、カレントディレクトリにある`DebugTrace.properties`ファイルを起動時に読み込みます。
`DebugTrace.properties`ファイルでは以下のプロパティを指定できます。

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
            &emsp;&emsp;<code>Logger = Diagnostics+Debug</code> - <i><code>System.Diagnostics.Debug</code> クラスを使用して出力</i><br>
            &emsp;&emsp;<code>Logger = Diagnostics+Trace</code> - <i><code>System.Diagnostics.Trace</code> クラスを使用して出力</i><br>
            &emsp;&emsp;<code>Logger = Log4net</code> - <i><code>Log4net</code> を使用して出力</i><br>
            &emsp;&emsp;<code>Logger = NLog</code> - <i><code>NLog</code> を使用して出力</i><br>
            <b>設定例(複数):</b><br>
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
            &emsp;&emsp;<code>LogLevel = Finer; Trace</code> - <i><code>Log4net</code> では <code>Finer </code>レベル、<code>NLog</code> では <code>Trace</code> レベルで出力</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>EnterFormat</code>
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
            <code>LeaveFormat</code>
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
            <code>ThreadBoundaryFormat</code></td>
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
            <code>ClassBoundaryFormat</code</td>
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
            <code>IndentString</code>
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
            <code>NonOutputString</code>
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
            <code>CountFormat</code>
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
            <code>MinimumOutputCount</code>
        </td>
        <td>
            コレクションの要素数を出力する最小値<br>
            <b>設定例:</b><br>
            &emsp;&emsp;<code>MinimumOutputCount = 5</code> <i>(デフォルト)</i>
        </td>
    </tr>
    <tr>
        <td>
            <code>LengthFormat</code>
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
            <code>MinimumOutputLength</code>
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
            <code>MaximumDataOutputWidth</code>
        </td>
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
            <code>NonOutputProperties</code>
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

以下の手順でプロジェクトに`DebugTrace.properties`ファイルを追加します。

1. プロジェクトのコンテキストメニューから`追加` - `新しい項目...`を選択する。
1. ダイアログで、`テキスト ファイル`を選択し、名前を`DebugTrace.properties`にして`追加`ボタンをクリックする。
1. 追加された`DebugTrace.properties`のコンテキストメニューから`プロパティ`を選択する。
1. `プロパティ`の`詳細`セクションにある`出力ディレクトリにコピー`の設定値を`新しい場合はコピーする`または`常にコピーする`に変更する。


### 7.2. **NonOutputProperties**, **NonOutputString**

DebugTrace use reflection to output object contenDebugTrace は、`ToString`メソッドが実装されていない場合は、リフレクションを使用してオブジェクト内容を出力します。
他のオブジェクトの参照があれば、そのオブジェクトの内容も出力します。
ただし循環参照がある場合は、自動的に検出して出力を中断します。  
`NonOutputProperties`プロパティを指定して出力を抑制する事もできます。
このプロパティの値は、カンマ区切りで複数指定できます。  
`NonOutputProperties`で指定されたプロパティの値は、`NonOutputString`で指定された文字列 (デフォルト: `***`) で出力されます。

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

## 7. ロギングライブラリの使用

You can output logs using the following libraries besides console output.

&emsp;&emsp;_logging Libraries_
<table>
    <tr><th>ロギングライブラリ</th><th>必要なパッケージ</th></tr>
    <tr><td>log4net</td><td>DebugTrace.Log4net</td></tr>
    <tr><td>NLog</td><td>DebugTrace.NLog</td></tr>
</table>

使用する場合は、上記パッケージを NuGet から追加してください。  
ロギングライブラリを使用する際の DebugTrace のロガー名は、`DebugTrace`です。

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
