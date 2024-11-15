using Godot;

namespace DeckBuilder;

public partial class StatsUI : HBoxContainer
{

	public HBoxContainer block;
	public Label blockLabel;
	public HealthUI health;

	public override void _Ready()
	{
		block = GetNode<HBoxContainer>("%Block");
		blockLabel = GetNode<Label>("%BlockLabel");
		health = GetNode<HealthUI>("%Health");
	}

	public void UpdateStats(Stats stats)
	{
		if (!IsInstanceValid(blockLabel)) return;
		if (!IsInstanceValid(health)) return;

		blockLabel.Text = stats.block.ToString();
		health.UpdateStats(stats);

		// block.Visible = stats.block > 0;
		block.Visible = true;
		health.Visible = stats.health > 0;
	}

}
