using DeckBuilder;

using Godot;

public partial class Shop : Control
{

    public Button button;

    public override void _Ready()
    {
        button = GetNode<Button>("%Button");

        button.Pressed += OnButtonPressed;
    }

    public void OnButtonPressed()
    {
        Events.Instance.EmitSignal(Events.SignalName.ShopExited);
    }

}
