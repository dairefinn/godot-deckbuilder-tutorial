namespace DeckBuilder;

using Godot;

public partial class Campfire : Control
{

    [Export] public CharacterStats charStats;

    public Button button;
    public AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        button = GetNode<Button>("%RestButton");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        button.Pressed += OnRestButtonPressed;
    }

    public void OnRestButtonPressed()
    {
        charStats.Heal(Mathf.CeilToInt(charStats.maxHealth * 0.3f));
        animationPlayer.Play("fade_out");
    }

    // This is called from the AnimationPlayer at the end of the 'fade_out' animation
    private void OnFadeOutFinished()
    {
        Events.Instance.EmitSignal(Events.SignalName.CampfireExited);
    }

}
