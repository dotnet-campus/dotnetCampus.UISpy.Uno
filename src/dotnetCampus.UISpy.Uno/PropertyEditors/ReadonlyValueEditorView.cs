namespace dotnetCampus.UISpy.Uno.PropertyEditors;

public class ReadonlyValueEditorView : UserControl
{
    public ReadonlyValueEditorView(object initialValue)
    {
        this.DataContext(
            new BindableObjectEditorModel().WithModel(x => x.Model.Value = initialValue),
            (v, vm) => v
                .Content(new TextBlock()
                    .Text(x => x.Binding(() => vm.Value))
                ));
    }
}

public partial record ObjectEditorModel
{
    public object? Value { get; set; }
}
