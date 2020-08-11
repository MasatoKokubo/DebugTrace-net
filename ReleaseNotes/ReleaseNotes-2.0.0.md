### Changed the supported framework to *.NET Core 3.1* from .NET Standard 2.0.

### Add following properties specified in DebugTrace.properties.
* `MinimumOutputCount` - Minimum value to output the number of elements of collection (Default: 5)
* `MinimumOutputLength` - Minimum value to output the length of string (Default: 5)

### Changed following property names specified in DebugTrace.properties. However, you can specify the previous names for compatibility.
* `EnterFormat` <- `EnterString`
* `LeaveFormat` <- `LeaveString`
* `IndentString` <- `CodeIndentString`
* `NonOutputString` <- `NonPrintString`
* `LengthFormat` <- `StringLengthFormat`
* `MaximumDataOutputWidth` <- `MaxDataOutputWidth`
* `NonOutputProperties` <- `NonPrintProperties`

### Improvement
* Improved the line break handling of data output.

<font color="blue">*The following are Japanese.*</font>

### 対応フレームワークを .NET Standard 2.0から *.NET Core 3.1* に変更。

### DebugTrace.propertiesで指定する以下のプロパティを追加
* `MinimumOutputCount` - コレクションの要素数を出力する最小値 (デフォルト: 5)
* `MinimumOutputLength` - 文字列長を出力する最小値 (デフォルト: 5)

### DebugTrace.propertiesで指定する以下のプロパティ名を変更 (互換性維持のため従来の名称も指定可能)
* `EnterFormat` <- `EnterString`
* `LeaveFormat` <- `LeaveString`
* `IndentString` <- `CodeIndentString`
* `NonOutputString` <- `NonPrintString`
* `LengthFormat` <- `StringLengthFormat`
* `MaximumDataOutputWidth` <- `MaxDataOutputWidth`
* `NonOutputProperties` <- `NonPrintProperties`

### 改善
* 改行の検出のアルゴリズムを変更して高速化。
