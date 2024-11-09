namespace DeckBuilder;

using Godot;

public partial class BattleOverPanel : Panel
{
	
	public enum Type {
		WIN,
		LOSE
	}

	public Label label;
	public Button continueButton;
	public Button restartButton;

    public override void _Ready()
    {
        label = GetNode<Label>("%Label");
		continueButton = GetNode<Button>("%ContinueButton");
		restartButton = GetNode<Button>("%RestartButton");

		continueButton.Pressed += OnContinueButtonPressed;
		restartButton.Pressed += OnRestartButtonPressed;

		Events.Instance.BattleOverScreenRequested += ShowScreen;
    }

	public void OnContinueButtonPressed()
	{
		Events.Instance.EmitSignal(Events.SignalName.BattleWon);
	}

	public void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}

	public void ShowScreen(string text, Type type)
	{
		if (!IsInstanceValid(label)) return;
		if (!IsInstanceValid(continueButton)) return;
		if (!IsInstanceValid(restartButton)) return;

		label.Text = text;
		continueButton.Visible = type == Type.WIN;
		restartButton.Visible = type == Type.LOSE;
		Show();
		GetTree().Paused = true;
	}

}
