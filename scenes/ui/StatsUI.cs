using Godot;

namespace DeckBuilder;

public partial class StatsUI : HBoxContainer
{

	public HBoxContainer block;
	public Label blockLabel;
	public HBoxContainer health;
	public Label healthLabel;

	public override void _Ready()
	{
		block = GetNode<HBoxContainer>("%Block");
		blockLabel = GetNode<Label>("%BlockLabel");
		health = GetNode<HBoxContainer>("%Health");
		healthLabel = GetNode<Label>("%HealthLabel");
	}

	public void UpdateStats(Stats stats)
	{
		if (!IsInstanceValid(blockLabel)) return;
		if (!IsInstanceValid(healthLabel)) return;

		blockLabel.Text = stats.block.ToString();
		healthLabel.Text = stats.health.ToString();

		block.Visible = stats.block > 0;
		health.Visible = stats.health > 0;
	}

}
