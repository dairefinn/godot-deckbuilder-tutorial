namespace DeckBuilder;

using Godot;

public partial class MainMenu : Control
{

    private PackedScene CHARACTER_SELECT_SCENE = GD.Load<PackedScene>("res://scenes/ui/character_selector.tscn");
    private PackedScene RUN_SCENE = GD.Load<PackedScene>("res://scenes/run/run.tscn");

    [Export] public RunStartup runStartup;

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

        ButtonContinue.Disabled = SaveGame.LoadData() == null;
    }

    public void OnContinuePressed()
    {
        runStartup.type = RunStartup.Type.CONTINUED_RUN;
        GetTree().ChangeSceneToPacked(RUN_SCENE);
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
