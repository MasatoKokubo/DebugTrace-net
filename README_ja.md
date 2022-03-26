# DebugTrace-net

[English](README.md)

**DebugTrace-net** は、.NETプログラムのデバッグ時にトレースログの出力をサポートするライブラリで、 
[.NET 6](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6) と
[.NET Core 3.1](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-core-3-1) に対応しています。

&emsp;&emsp;_DebugTrace-net のバージョンと .NET / .NET Core のバージョンの対応_

|DebugTrace-net のバージョン|.NET / .NET Core のバージョン|
|:----|:----|
|3.x.x<br>&emsp;_Visual Basicには非対応_|.NET 6.0|
|2.x.x|.NET Core 3.1

## 1. 特徴

* ログの行末に呼び出し元のクラス名、 メソッド名 、 ソースファイル名および行番号を自動的に出力。
* メソッドやオブジェクトのネストで、ログを自動的にインデント。
* スレッドの切り替え時に自動的にログを出力。
* `ToString` メソッドを実装していないクラスのオブジェクトでもリフレクションを使用して内容を出力。
* `DebugTrace.properties` ファイルにプロパティを設定する事で、出力内容のカスタマイズが可能。
* 依存するライブラリがない (コンソールに出力する場合)。
* 各種ロギングライブラリを使用可能。
    * https://logging.apache.org/log4net/[log4net]
    * http://nlog-project.org/[NLog]

## 2. インストール
NuGetから `DebugTrace` パッケージを検索してインストールしてください。  
[log4net](https://logging.apache.org/log4net/) または [NLog](http://nlog-project.org/) を使用してログを出力する場合は、`DebugTrace.Log4net` または `DebugTrace.NLog` をインストールしてください。

## 3. 使用方法

デバッグ対象および関連するメソッドに対して以下を行います。

1. メソッドの先頭に `Trace.Enter()` を挿入する。
1. メソッドの終了(または `return` 文の直前)に `Trace.Leave()` を挿入する。
1. 必要に応じて変数をログに出力する `Trace.Print("foo", foo)` を挿入する。

[詳細...](README_ja_details.md)

## 9. ライセンス

[MIT ライセンス(MIT)](LICENSE)

## 10. リリースノート

### DebugTrace-net 3.0.0 - _2022/5/22_

* `.NET 6.0` に対応しました。
* `Print` メソッドは、引数の値またはメッセージを返すようにしました。

<small><i>関連パッケージ:</i></small>
<table>
    <tr><td>DebugTrace.Log4net 3.0.0</td><td>log4net 2.0.14</td></tr>
    <tr><td>DebugTrace.NLog 3.0.0</td><td>NLog 4.7.15</td></tr>
</table>

### DebugTrace-net 2.1.0 - _2022/5/22_

* `Print` メソッドは、引数の値またはメッセージを返すようにしました。

<small><i>関連パッケージ:</i></small>
<table>
    <tr><td>DebugTrace.Log4net 2.1.0</td><td>log4net 2.0.14</td></tr>
    <tr><td>DebugTrace.NLog 2.1.0</td><td>NLog 4.7.15</td></tr>
</table>

[すべてのリリースノート...](README_ja_releaseNotes.md)

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
