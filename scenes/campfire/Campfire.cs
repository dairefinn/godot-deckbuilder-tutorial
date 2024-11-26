namespace DeckBuilder;

using Godot;

public partial class Campfire : Control
{

    [Export] public CharacterStats charStats;

    public Button restButton;
    public AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        restButton = GetNode<Button>("%RestButton");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        restButton.Pressed += OnRestButtonPressed;
    }

    public void OnRestButtonPressed()
    {
        restButton.Disabled = true;
        charStats.Heal(Mathf.CeilToInt(charStats.maxHealth * 0.3f));
        animationPlayer.Play("fade_out");
    }

    // This is called from the AnimationPlayer at the end of the 'fade_out' animation
    private void OnFadeOutFinished()
    {
        Events.Instance.EmitSignal(Events.SignalName.CampfireExited);
    }

}
