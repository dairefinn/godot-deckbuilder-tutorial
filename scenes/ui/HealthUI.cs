namespace DeckBuilder;

using Godot;

public partial class HealthUI : HBoxContainer
{

    // [Export] public CharacterStats charStats;
    [Export] public bool showMaxHealth;

    public Label HealthLabel;
    public Label MaxHealthLabel;

    public override void _Ready()
    {
        HealthLabel = GetNode<Label>("HealthLabel");
        MaxHealthLabel = GetNode<Label>("MaxHealthLabel");
    }

    public void UpdateStats(Stats stats)
    {
        if (!IsInstanceValid(HealthLabel)) return;
        if (!IsInstanceValid(MaxHealthLabel)) return;

        HealthLabel.Text = stats.health.ToString();
        MaxHealthLabel.Text = "/" + stats.maxHealth.ToString();

        MaxHealthLabel.Visible = showMaxHealth;
    }

}
