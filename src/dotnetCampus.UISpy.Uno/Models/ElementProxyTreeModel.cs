using dotnetCampus.UISpy.Uno.Tree;

namespace dotnetCampus.UISpy.Uno.Models;

public partial record ElementProxyTreeModel
{
    public IState<UIElement> CurrentRootElement => State<UIElement>.Empty(this);

    public IFeed<ElementProxyTree> CurrentTree => Feed.Async(UpdateVisualTree);

    public void SetRootElement(UIElement rootElement)
    {
    }

    private ValueTask<ElementProxyTree> UpdateVisualTree(CancellationToken ct)
    {
        return ValueTask.FromResult<ElementProxyTree>(default);
    }
}
