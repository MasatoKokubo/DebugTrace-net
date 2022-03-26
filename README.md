# DebugTrace-net

[Japanese](README_ja.md)

**DebugTrace-net** is a library that outputs trace logs when debugging .NET programs.
It based on [.NET 6](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6) and
[.NET Core 3.1](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-core-3-1).

&emsp;&emsp;_Correspondence between .DebugTrace-net versions and .NET or .NET Core versions_

|DebugTrace-net version|.NET or .NET Core version|
|:----|:----|
|3.x.x<br>&emsp;_Not compatible with Visual Basic_|.NET 6.0|
|2.x.x|.NET Core 3.1|

## 1. Features

* Automatically outputs invoker's class name, method name, source file name and line number.
* Automatically indents the log with nesting methods and objects.
* Automatically breaks at the output of values.
* Automatically output logs when changing threads.
* Uses reflection to output objects of classes that do not implement `ToString` method.
* You can customize the output content in `DebugTrace.properties`.
* There are no dependent libraries at run time if you output to the console.
* You can use the following logging library.
    * [log4net](ttps://logging.apache.org/log4net/)
    * [NLog](http://nlog-project.org/)

## 2. Install

Search `DebugTrace` packege on NuGet and install it.  
If you output logs using [log4net](https://logging.apache.org/log4net/) or [NLog](http://nlog-project.org/),
install `DebugTrace.Log4net` or `DebugTrace.NLog` package.

## 3. How to use

Do the following for debug target and related methods.

1. Insert `Trace.Enter()` at the beginning of methods.
1. Insert `Trace.Leave()` at the end of methods or just before the `return` statements.
1. Insert `Trace.Print("foo", foo)` to output arguments, local variables and return value to the log if necessary.

[Details...](README_details.md)

## 9. License

[The MIT License (MIT)](LICENSE)

## 10. Release Notes

### DebugTrace-net 3.0.0 - _May 22, 2022_

* Support for .NET 6.0.
* `Print` methods now returns the value or the message of the argument.

_Related packages:_
<table>
    <tr><td>DebugTrace.Log4net 3.0.0</td><td>log4net 2.0.14</td></tr>
    <tr><td>DebugTrace.NLog 3.0.0</td><td>NLog 4.7.15</td></tr>
</table>

### DebugTrace-net 2.1.0 - _May 22, 2022_

* `Print` methods now returns the value or the message of the argument.

_Related packages:_
<table>
    <tr><td>DebugTrace.Log4net 2.1.0</td><td>log4net 2.0.14</td></tr>
    <tr><td>DebugTrace.NLog 2.1.0</td><td>NLog 4.7.15</td></tr>
</table>

[All Release Notes...](README_releaseNotes.md)

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
