namespace DeckBuilder;

using Godot;

public partial class MainMenu : Control
{

    private PackedScene CHARACTER_SELECT_SCENE = GD.Load<PackedScene>("res://scenes/ui/character_selector.tscn");

    public Button ButtonContinue;
    public Button ButtonNewRun;
    public Button ButtonExit;

    public override void _Ready()
    {
        GetTree().Paused = false;

        ButtonContinue = GetNode<Button>("%Continue");
        ButtonNewRun = GetNode<Button>("%NewRun");
        ButtonExit = GetNode<Button>("%Exit");

        ButtonContinue.Pressed += OnContinuePressed;
        ButtonNewRun.Pressed += OnNewRunPressed;
        ButtonExit.Pressed += OnExitPressed;
    }

    public void OnContinuePressed()
    {
        GD.Print("Continue run");
    }

    public void OnNewRunPressed()
    {
        GetTree().ChangeSceneToPacked(CHARACTER_SELECT_SCENE);
    }

    public void OnExitPressed()
    {
        GetTree().Quit();
    }

}
