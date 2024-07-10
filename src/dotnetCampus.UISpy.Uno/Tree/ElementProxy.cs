using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;

namespace dotnetCampus.UISpy.Uno.Tree;

[DebuggerDisplay("{TypeName,nq} ({Name,nq}) [{Children.Length,nq}]")]
public readonly record struct ElementProxy(DependencyObject Element, ImmutableArray<ElementProxy> Children)
{
    public DependencyObject Element { get; init; } = Element;

    public string TypeName { get; } = Element.GetType().Name;

    public string FullTypeName { get; } = Element.GetType().FullName!;

    public string? Name { get; } = Element.GetValue(FrameworkElement.NameProperty) as string ?? Element.GetType().Name;

    public ImmutableArray<ElementPropertyProxy> GetProperties()
    {
        var properties = new List<ElementPropertyProxy>();
        var propertyDescriptors = TypeDescriptor.GetProperties(Element);

        foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
        {
            try
            {
                object? value;
                if (propertyDescriptor.Name == "ActualWidth")
                {

                }
                value = propertyDescriptor.GetValue(Element);
                properties.Add(new ElementPropertyProxy(
                    Element,
                    propertyDescriptor.Name,
                    value,
                    propertyDescriptor.PropertyType.Name));
            }
            catch (Exception ex)
            {
                properties.Add(new ElementPropertyProxy(
                    Element,
                    propertyDescriptor.Name,
                    ex,
                    propertyDescriptor.PropertyType.Name)
                {
                    IsFailed = true,
                });
            }
        }

        return [.. properties];
    }

    public static ElementProxy Create(DependencyObject element, List<ElementProxy> children)
    {
        return new ElementProxy(element, [.. children]);
    }
}
