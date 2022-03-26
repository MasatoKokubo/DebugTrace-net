# DebugTrace-net

[Japanese](README_ja.md)

**DebugTrace-net** is a library that outputs trace logs when debugging .NET programs,
and can be used with programs using [.NET Core 3.1](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-core-3-1) or later.

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

### DebugTrace-net 2.1.0 - _November 13, 2022_

* `Print` methods now returns the value or the message of the argument.
* Now prints the runtime `.NET` version in the `DbeugTrace-net` startup log.

_Related packages:_

|DebugTrace Package|Related Package|
|:----|:----|
|DebugTrace.Log4net 2.1.0|log4net 2.0.15|
|DebugTrace.NLog 2.1.0   |NLog 4.7.15|

[All Release Notes...](README_releaseNotes.md)

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
