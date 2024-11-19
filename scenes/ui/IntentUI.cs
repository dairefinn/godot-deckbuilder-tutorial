namespace DeckBuilder;

using Godot;

public partial class IntentUI : HBoxContainer
{

    public TextureRect icon;
    public Label label;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        label = GetNode<Label>("Label");
    }

    public void UpdateIntent(Intent intent)
    {
        if (intent == null)
        {
            Hide();
            return;
        }

        icon.Texture = intent.icon;
        icon.Visible = icon.Texture != null;
        label.Text = string.Format("{0}", intent.currentText);
        label.Visible = intent.currentText != null;
    }

}
