# DebugTrace-net

[English](README.md)

**DebugTrace-net** は、.NETプログラムのデバッグ時にトレースログの出力をサポートするライブラリで、 
[.NET 6](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6) 以降を使用しているプログラムで使用できます。


## 1. 特徴

* ログの行末に呼び出し元のクラス名、 メソッド名 、 ソースファイル名および行番号を自動的に出力。
* メソッドやオブジェクトのネストで、ログを自動的にインデント。
* スレッドの切り替え時に自動的にログを出力。
* `ToString`メソッドを実装していないクラスのオブジェクトでもリフレクションを使用して内容を出力。
* `DebugTrace.properties`ファイルにプロパティを設定する事で、出力内容のカスタマイズが可能。
* 依存するライブラリがない (コンソールに出力する場合)。
* 各種ロギングライブラリを使用可能。
    * [log4net](https://logging.apache.org/log4net/)
    * [NLog](https://nlog-project.org/)

## 2. インストール
NuGetから`DebugTrace`パッケージを検索してインストールしてください。  
[log4net](https://logging.apache.org/log4net/) または [NLog](http://nlog-project.org/) を使用してログを出力する場合は、`DebugTrace.Log4net`または`DebugTrace.NLog`をインストールしてください。

## 3. 使用方法

デバッグ対象および関連するメソッドに対して以下を行います。

1. メソッドの先頭に`Trace.Enter()`を挿入する。
1. メソッドの終了(または`return`文の直前)に`Trace.Leave()`を挿入する。
1. 必要に応じて変数をログに出力する`Trace.Print("foo", foo)`を挿入する。

[詳細...](README_ja_details.md)

## 8. ライセンス

[MIT ライセンス(MIT)](LICENSE)

## 9. リリースノート

### DebugTrace-net 3.0.0 - _2024/6/23_

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

[すべてのリリースノート...](README_ja_releaseNotes.md)

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
