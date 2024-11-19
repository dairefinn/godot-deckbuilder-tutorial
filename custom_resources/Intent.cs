namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class Intent : Resource
{

    [Export] public string baseText;
    [Export] public Texture2D icon;

    public string currentText;

}
