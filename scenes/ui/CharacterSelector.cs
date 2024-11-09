namespace DeckBuilder;

using Godot;

public partial class CharacterSelector : Control
{

    private readonly PackedScene RUN_SCENE = GD.Load<PackedScene>("res://scenes/run/run.tscn");
    private readonly CharacterStats WARRIOR_STATS = GD.Load<CharacterStats>("res://characters/warrior/Warrior.tres");
    private readonly CharacterStats WIZARD_STATS = GD.Load<CharacterStats>("res://characters/wizard/Wizard.tres");
    private readonly CharacterStats ASSASSIN_STATS = GD.Load<CharacterStats>("res://characters/assassin/Assassin.tres");

    [Export] public RunStartup runStartup;

    public Button StartButton;
    public TextureRect CharacterPortrait;
    public Label Title;
    public Label Description;

    public Button WarriorButton;
    public Button WizardButton;
    public Button AssassinButton;

    public CharacterStats currentCharacter {
        get => _currentCharacter;
        set => SetCurrentCharacter(value);
    }
    private CharacterStats _currentCharacter;

    public override void _Ready()
    {
        StartButton = GetNode<Button>("%StartButton");
        CharacterPortrait = GetNode<TextureRect>("%CharacterPortrait");
        Title = GetNode<Label>("%Title");
        Description = GetNode<Label>("%Description");

        WarriorButton = GetNode<Button>("%WarriorButton");
        WizardButton = GetNode<Button>("%WizardButton");
        AssassinButton = GetNode<Button>("%AssassinButton");

        currentCharacter = WARRIOR_STATS;

        WarriorButton.Pressed += OnWarriorButtonPressed;
        WizardButton.Pressed += OnWizardButtonPressed;
        AssassinButton.Pressed += OnAssassinButtonPressed;

        StartButton.Pressed += OnStartButtonPressed;
    }

    public void OnStartButtonPressed()
    {
        GD.Print("Starting a new run with the " + currentCharacter.characterName);
        runStartup.type = RunStartup.Type.NEW_RUN;
        runStartup.pickedCharacter = currentCharacter;
        GetTree().ChangeSceneToPacked(RUN_SCENE);

    }

    public void OnWarriorButtonPressed()
    {
        currentCharacter = WARRIOR_STATS;
    }

    public void OnWizardButtonPressed()
    {
        currentCharacter = WIZARD_STATS;
    }

    public void OnAssassinButtonPressed()
    {
        currentCharacter = ASSASSIN_STATS;
    }

    private void SetCurrentCharacter(CharacterStats character)
    {
        _currentCharacter = character;
        Title.Text = character.characterName;
        Description.Text = character.description;
        CharacterPortrait.Texture = character.portrait;
    }

}
