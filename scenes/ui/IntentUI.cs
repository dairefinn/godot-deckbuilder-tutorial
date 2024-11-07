namespace DeckBuilder;

using Godot;

public partial class IntentUI : HBoxContainer
{

    public TextureRect icon;
    public Label number;

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
        number = GetNode<Label>("Number");
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

        bool isNumber = int.TryParse(intent.number, out int intentNumberParsed);
        if (!isNumber)
        {
            number.Visible = false;
        }
        else
        {
            number.Text = string.Format("{0}", intentNumberParsed);
            number.Visible = true;
        }
    }

}
