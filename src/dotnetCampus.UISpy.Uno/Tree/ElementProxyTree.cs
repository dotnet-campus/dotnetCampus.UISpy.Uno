namespace dotnetCampus.UISpy.Uno.Tree;

public class ElementProxyTree(ElementProxy root)
{
    public ElementProxy Root => root;

    public static ElementProxyTree BuildVisualTree(DependencyObject root)
    {
        return new ElementProxyTree(BuildVisualTreeNode(root));
    }

    private static ElementProxy BuildVisualTreeNode(DependencyObject element)
    {
        var children = new List<ElementProxy>();
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            var childNode = BuildVisualTreeNode(child);
            children.Add(childNode);
        }
        return ElementProxy.Create(element, children);
    }
}
