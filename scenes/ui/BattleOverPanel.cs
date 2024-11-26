namespace DeckBuilder;

using Godot;

public partial class BattleOverPanel : Panel
{

	public static readonly string MAIN_MENU = "res://scenes/ui/main_menu.tscn";
	
	public enum Type {
		WIN,
		LOSE
	}

	public Label label;
	public Button continueButton;
	public Button mainMenuButton;

    public override void _Ready()
    {
        label = GetNode<Label>("%Label");
		continueButton = GetNode<Button>("%ContinueButton");
		mainMenuButton = GetNode<Button>("%MainMenuButton");

		continueButton.Pressed += OnContinueButtonPressed;
		mainMenuButton.Pressed += OnMainMenuButtonPressed;

		Events.Instance.BattleOverScreenRequested += ShowScreen;
    }

	public void OnContinueButtonPressed()
	{
		Events.Instance.EmitSignal(Events.SignalName.BattleWon);
	}

	public void OnMainMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile(MAIN_MENU);
	}

	public void ShowScreen(string text, Type type)
	{
		if (!IsInstanceValid(label)) return;
		if (!IsInstanceValid(continueButton)) return;
		if (!IsInstanceValid(mainMenuButton)) return;

		label.Text = text;
		continueButton.Visible = type == Type.WIN;
		mainMenuButton.Visible = type == Type.LOSE;
		Show();
		GetTree().Paused = true;
	}

}
