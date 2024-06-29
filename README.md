# DebugTrace-net

[Japanese](README_ja.md)

**DebugTrace-net** is a library that outputs trace logs when debugging .NET programs,
and can be used with programs using [.NET 6](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6) or later.

## 1. Features

* Automatically outputs invoker's class name, method name, source file name and line number.
* Automatically indents the log with nesting methods and objects.
* Automatically breaks at the output of values.
* Automatically output logs when changing threads.
* Uses reflection to output objects of classes that do not implement `ToString` method.
* You can customize the output content in `DebugTrace.properties`.
* There are no dependent libraries at run time if you output to the console.
* You can use the following logging library.
    * [log4net](https://logging.apache.org/log4net/)
    * [NLog](https://nlog-project.org/)

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

## 8. License

[The MIT License (MIT)](LICENSE)

## 9. Release Notes

### DebugTrace-net 3.0.0 - _June 23, 2022_

* Now compatible with .NET 6.
* Changed the default values of the following properties in DebugTrace.properties.

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

|Property Name|New Default Value|Old Default Value|
|:----------- |:---------------:|:---------------:|
|MinimumOutputCount |128|   5|
|MinimumOutputLength|256|   5|
|CollectionLimit    |128| 512|
|StringLimit        |256|8192|

_Related packages:_

|DebugTrace Package|Related Package|
|:----|:----|
|DebugTrace.Log4net 3.0.0|log4net 2.0.17|
|DebugTrace.NLog 3.0.0   |NLog 5.2.8|

[All Release Notes...](README_releaseNotes.md)

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
