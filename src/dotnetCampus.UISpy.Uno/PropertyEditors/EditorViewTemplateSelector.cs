using dotnetCampus.UISpy.Uno.Tree;

//using Uno.Extensions.Reactive.Bindings;

namespace dotnetCampus.UISpy.Uno.PropertyEditors;

public class EditorViewTemplateSelector : DataTemplateSelector
{
    protected override DataTemplate SelectTemplateCore(object item)
    {
        var proxy = (ElementPropertyProxy)item;
        UIElement view = proxy.PropertyTypeName switch
        {
            nameof(Boolean) => new BooleanEditorView(proxy.Value is true),
            _ => new ReadonlyValueEditorView(proxy.Value?.ToString()),
        };
#if HAS_UNO
        return new DataTemplate(() => view);
#else
        return null!;
#endif
    }
}

//internal static class EditorViewExtensions
//{
//    public static TBindable WithModel<TBindable>(this TBindable bindableModel, Action<TBindable> setter)
//        where TBindable : BindableViewModelBase
//    {
//        setter(bindableModel);
//        return bindableModel;
//    }
//}
