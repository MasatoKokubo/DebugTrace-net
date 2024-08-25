### 4.0.0 - _2025/6/28_

* .NET 8 に対応しました。

* `Print`メソッドの以下の引数名を変更しました。
    * reflection ← forceReflection
    * reflectionLimit ← reflectionNestLimit

* `DebugTrace.properties`の以下のプロパティ名を変更しました。
    * ReflectionLimit ← ReflectionNestLimit

<small><i>関連パッケージ:</i></small>

|DebugTraceパッケージ|関連パッケージ|
|:-----------------|:-------------|
|DebugTrace.Log4net 4.0.0|log4net 3.1.0|
|DebugTrace.NLog 4.0.0   |NLog 6.0.1|

### 3.0.0 - _2024/6/23_

* .NET 6に対応しました。
* DebugTrace.propertiesの以下のプロパティのデフォルト値を変更しました。

| プロパティ名|新デフォルト値|旧デフォルト値|
|:----------|:----------:|:----------:|
|MinimumOutputCount |128|   5|
|MinimumOutputLength|256|   5|
|CollectionLimit    |128| 512|
|StringLimit        |256|8192|

* VisualStudioのサポートを廃止しました。
    これに伴い以下を行いました。
    * `ITrace`インタフェースの削除
    * `CSharp`および`VisualBasic`クラスの削除
    * `TraceBase`のクラス名を`Trace`に変更  

    `using`の記述は以下のように変わります。  
    `using static DebugTrace.CSharp;`  
        ↓  
    `using DebugTrace;`

* メソッドの引数の仕様変更  
    * `T? Print<T>(string name, T? value)`  
        ↓  
    `T? Print<T>(string name, T? value, bool reflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionLimit = -1)`

    * `T? Print<T>(string name, Func<T?> valueSupplier)`  
        ↓  
    `T? Print<T>(string name, Func<T?> valueSupplier, bool reflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionLimit = -1)`

<small><i>関連パッケージ:</i></small>

|DebugTraceパッケージ|関連パッケージ|
|:-----------------|:-------------|
|DebugTrace.Log4net 3.0.0|log4net 2.0.17|
|DebugTrace.NLog 3.0.0   |NLog 5.2.8|

### 2.1.0 - _2022/11/13_

* `Print`メソッドは、引数の値またはメッセージを返すようにしました。
* `DebugTrace-net`の開始時のログにランタイムの`.NET`バージョンを出力するようにしました。

<small><i>関連パッケージ:</i></small>

|DebugTraceパッケージ|関連パッケージ|
|:----|:----|
|DebugTrace.Log4net 2.1.0|log4net 2.0.15|
|DebugTrace.NLog 2.1.0   |NLog 4.7.15|

### 2.0.3 - _2021/8/13_

* データ出力の改行処理を改善しました。

### 2.0.2 - _2020/7/12_

* データ出力の改行処理を改善しました。

### 2.0.1 - _2020/5/16_

* データ出力の改行処理を改善しました。

### 2.0.0 - _2020/4/4_

* 対応フレームワークを .NET Standard 2.0 から .NET Core 3.1 に変更しました。

* DebugTrace.propertiesで指定する以下のプロパティを追加
    * `MinimumOutputCount` - コレクションの要素数を出力する最小値 (デフォルト: 5)
    * `MinimumOutputLength` - 文字列長を出力する最小値 (デフォルト: 5)

* DebugTrace.propertiesで指定する以下のプロパティ名を変更 (互換性維持のため従来の名称も指定可能)
    * `EnterFormat` <- `EnterString`
    * `LeaveFormat` <- `LeaveString`
    * `IndentString` <- `CodeIndentString`
    * `NonOutputString` <- `NonPrintString`
    * `LengthFormat` <- `StringLengthFormat`
    * `MaximumDataOutputWidth` <- `MaxDataOutputWidth`
    * `NonOutputProperties` <- `NonPrintProperties`

* 改善
    * 改行の検出のアルゴリズムを変更して高速化しました。

### 1.6.0 - _2019/3/24_

* 以下のロガーを追加。
    * Diagnostics+Debug
    * Diagnostics+Trace

* `Trace`クラスに`PrintStack(int)`メソッドを追加。

### 1.5.4 - _2019/2/11_

* `Print`メソッドの変更
    * 型名の前に`enum`の表示  
      例: `v = enum Fruits Apple`

* `Print`メソッドの改善
    * プロパティまたはフィールドの型とその値の型が異なる場合は、プロパティまたはフィールド名の前に型名を出力する。

### 1.5.3 - _2019/2/3_

* `Print`メソッドの改善
    * 型名の後に`struct`の表示を追加。  
      例: `v = Point struct {X: 1, Y: 2}`
    * 型名の後に`enum`の表示を追加。  
      例: `v = Fruits enum Apple`

### 1.5.2 - _2019/1/28_

* Add `Trace_` property to `CSharp` and `VisualBasic` classes.

### 1.5.1 - _2018/12/15_

* 改善
    * 文字列の長さを出力するようになりました。

* Add Properties in `DebugTrace.properties`
    * `CountFormat`: The format string of the count of collections
    * `StringLengthFormat`: The format string of the length of strings

### DebugTrace.NLog 1.6.0 - _2018/11/18_
* 対応フレームワークを.NET Frameword 4.7から.NET Standard 2.0に変更。

### 1.5.0 - _2018/10/28_
* バグ修正
    * **_[修正済]_** `Trace.OutputNonPublicFields = true`の場合に`Task`を出力すると`NullReferenceException`がスローされる。

* 改善
    * DebugTrace.propertiesで複数のロガーを指定できるようになりました。(例: `Logger = Console+Out; Log4net`)

### DebugTrace.Log4net 1.5.0 - _2018/10/28_
* DebugTrace-net 1.5.0 に対応するリリース

### DebugTrace.NLog 1.5.0 - _2018/10/28_
* 変更
    * DebugTrace-net 1.5.0に対応するリリース
    * Nlog 4.5.10に依存

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
