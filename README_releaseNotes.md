## 11. Release Notes

### DebugTrace-net 2.1.0 - _November 13, 2022_

* `Print` methods now returns the value or the message of the argument.
* Now prints the runtime `.NET` version in the `DbeugTrace-net` startup log.

_Related packages:_

|DebugTrace Package|Related Package|
|:----|:----|
|DebugTrace.Log4net 2.1.0|log4net 2.0.15|
|DebugTrace.NLog 2.1.0   |NLog 4.7.15|

### DebugTrace-net 2.0.3 - _August 13, 2021_

* Improved the line break handling of data output.

### DebugTrace-net 2.0.2 - _July 12, 2020_

* Improved the line break handling of data output.

### DebugTrace-net 2.0.1 - _May 16, 2020_

* Improved the line break handling of data output.

### DebugTrace-net 2.0.0 - _April 4, 2020_

* Changed the supported framework to .NET Core 3.1 from .NET Standard 2.0.

* Add following properties specified in DebugTrace.properties.
    * `MinimumOutputCount` - The minimum value to output the number of elements of collection (Default: 5)
    * `MinimumOutputLength` - The minimum value to output the length of string (Default: 5)

* Changed following property names specified in DebugTrace.properties. However, you can specify the previous names for compatibility.
    * `EnterFormat` <- `EnterString`
    * `LeaveFormat` <- `LeaveString`
    * `IndentString` <- `CodeIndentString`
    * `NonOutputString` <- `NonPrintString`
    * `LengthFormat` <- `StringLengthFormat`
    * `MaximumDataOutputWidth` <- `MaxDataOutputWidth`
    * `NonOutputProperties` <- `NonPrintProperties`

* Improvement
    * Speed up by changing the algorithm of line break detection.

### DebugTrace-net 1.6.0 - _March 24, 2019_

* Add following loggers.
    * Diagnostics+Debug
    * Diagnostics+Trace

* Add `PrintStack(int)` method to `TraceBase` class.

### DebugTrace-net 1.5.4 - _February 11, 2019_

* Change of `Print` method
    * Outputs " enum" before the type name.  
      e.g.: `v = enum Fruits Apple`

* Improvement of `Print` method
    * Outputs the type name before the property or field name if the value type is different from the property or field type.

### DebugTrace-net 1.5.3 - _February 3, 2019_

* Improvements of `Print` method
    * Outputs `struct` after the type name.  
      e.g.: `v = Point struct {X: 1, Y: 2}`
    * Outputs `enum` after the type name.  
      e.g.: `v = Fruits enum Apple`

### DebugTrace-net 1.5.2 - _January 28, 2019_

* Add `Trace_` property to `CSharp` and `VisualBasic` classes.

### DebugTrace-net 1.5.1 - _December 15, 2018_

* Improvement
    * Now outputs the length of strings.

* Add Properties in `DebugTrace.properties`
    * `CountFormat`: The format string of the count of collections
    * `StringLengthFormat`: The format string of the length of strings

### DebugTrace.NLog 1.6.0 - _November 18, 2018_
* Changed target framework from .NET Frameword 4.7 to .NET Standard 2.0.

### DebugTrace-net 1.5.0 - _October 28, 2018_
* Bug fix
    * **_[Fixed]_** Throws a `NullReferenceException` when print a `Task` on `TraceBase.OutputNonPublicFields = true`.

* Improvement
    * You can now specify multiple loggers in DebugTrace.properties. (e.g.: `Logger = Console+Out; Log4net`)

### DebugTrace.Log4net 1.5.0 - _October 28, 2018_
* This release is for DebugTrace-net 1.5.0.

### DebugTrace.NLog 1.5.0 - _October 28, 2018_
* Changes
    * This release is for DebugTrace-net 1.5.0.
    * Depends on Nlog 4.5.10.

<div align="center" style="color:#6699EE">(C) 2018 Masato Kokubo</div>
