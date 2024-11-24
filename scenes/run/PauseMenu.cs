namespace DeckBuilder;

using Godot;

public partial class PauseMenu : CanvasLayer
{

	[Signal] public delegate void SaveAndQuitEventHandler();

	public Button BackToGameButton;
	public Button SaveAndQuitButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BackToGameButton = GetNode<Button>("%BackToGameButton");
		SaveAndQuitButton = GetNode<Button>("%SaveAndQuitButton");

		BackToGameButton.Pressed += Unpause;
		SaveAndQuitButton.Pressed += OnSaveAndQuitButtonPressed;
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("pause"))
		{
			if (Visible)
			{
				Unpause();
			}
			else
			{
				Pause();
			}

			GetViewport().SetInputAsHandled();
		}
    }

	public void Pause()
	{
		Show();
		GetTree().Paused = true;
	}

	public void Unpause()
	{
		Hide();
		GetTree().Paused = false;
	}

	public void OnSaveAndQuitButtonPressed()
	{
		GetTree().Paused = false;
		EmitSignal(SignalName.SaveAndQuit);
	}

}
