* Now compatible with .NET 6.
* Changed the default values of the following properties in DebugTrace.properties.

|Property Name|New Default Value|Old Default Value|
|:----------- |:---------------:|:---------------:|
|MinimumOutputCount |128|   5|
|MinimumOutputLength|256|   5|
|CollectionLimit    |128| 512|
|StringLimit        |256|8192|

* Support for VisualStudio has been dropped.
    As a result, the following has been done:
    * The `ITrace` interface has been removed.
    * The `CSharp` and `VisualBasic` classes have been removed.
    * The `TraceBase` class name has been changed to `Trace`.
 
    The `using` statement will change to the following: 
    `using static DebugTrace.CSharp;`  
        ↓  
    `using DebugTrace;`

* Changes to method argument specifications    
    * `T? Print<T>(string name, T? value)`  
        ↓  
    `T? Print<T>(string name, T? value, bool forceReflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionNestLimit = -1)`

    * `T? Print<T>(string name, Func<T?> valueSupplier)`  
        ↓  
    `T? Print<T>(string name, Func<T?> valueSupplier, bool forceReflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionNestLimit = -1)`

_Related packages:_

|DebugTrace Package|Related Package|
|:----|:----|
|DebugTrace.Log4net 3.0.0|log4net 2.0.17|
|DebugTrace.NLog 3.0.0   |NLog 5.2.8|

---
*Japanese*

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
    `T? Print<T>(string name, T? value, bool forceReflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionNestLimit = -1)`

    * `T? Print<T>(string name, Func<T?> valueSupplier)`  
        ↓  
    `T? Print<T>(string name, Func<T?> valueSupplier, bool forceReflection = false,`  
    `    bool? outputNonPublicFields = null, bool? putNonPublicProperties = null,`  
    `    int minimumOutputCount = -1, int minimumOutputLength = -1,`  
    `    int collectionLimit = -1, int stringLimit = -1, int reflectionNestLimit = -1)`

<small><i>関連パッケージ:</i></small>

|DebugTraceパッケージ|関連パッケージ|
|:-----------------|:-------------|
|DebugTrace.Log4net 3.0.0|log4net 2.0.17|
|DebugTrace.NLog 3.0.0   |NLog 5.2.8|
