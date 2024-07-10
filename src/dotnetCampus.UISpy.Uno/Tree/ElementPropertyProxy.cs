namespace dotnetCampus.UISpy.Uno.Tree;

public readonly record struct ElementPropertyProxy(
    DependencyObject Element,
    string PropertyName,
    object? Value,
    string PropertyTypeName)
{
    public bool IsFailed { get; init; }

    public bool IsNotImplemented { get; init; }
}
