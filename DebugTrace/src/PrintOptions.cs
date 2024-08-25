// PrintOptions.cs
// (C) 2018 Masato Kokubo
namespace DebugTrace;

/// <summary>
/// Have print options.
/// </summary>
/// <since>3.0.0</since>
/// <author>Masato Kokubo</author>
internal record class PrintOptions(
    bool Reflection,
    bool OutputNonPublicFields,
    bool OutputNonPublicProperties,
    int MinimumOutputCount,
    int MinimumOutputLength,
    int CollectionLimit,
    int StringLimit,
    int ReflectionLimit
);
