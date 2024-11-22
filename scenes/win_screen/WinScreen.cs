namespace DeckBuilder;

using Godot;

public partial class WinScreen : Control
{

    public static readonly string MAIN_MENU_PATH = "res://scenes/ui/main_menu.tscn";
    public static readonly string MESSAGE = "The {0}\nis victorious!";

    [Export] public CharacterStats character {
        get => _character;
        set => SetCharacter(value);
    }
    private CharacterStats _character;

    public Button mainMenuButton;
    public Label message;
    public TextureRect characterPortrait;

    public override void _Ready()
    {
        mainMenuButton = GetNode<Button>("%MainMenuButton");
        message = GetNode<Label>("%Message");
        characterPortrait = GetNode<TextureRect>("%CharacterPortrait");

        mainMenuButton.Pressed += OnMainMenuButtonPressed;
    }

    public void SetCharacter(CharacterStats value)
    {
        _character = value;
        message.Text = string.Format(MESSAGE, character.characterName);
        characterPortrait.Texture = character.portrait;
    }

    public void OnMainMenuButtonPressed()
    {
        GetTree().ChangeSceneToFile(MAIN_MENU_PATH);
    }

}
