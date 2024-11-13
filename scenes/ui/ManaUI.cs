namespace DeckBuilder;

using Godot;

public partial class ManaUI : Panel
{

	[Export] public CharacterStats charStats {
		get => _charStats;
		set => SetCharStats(value);
	}
	private CharacterStats _charStats;

	public Label manaLabel;

	public override void _Ready()
	{
		manaLabel = GetNode<Label>("ManaLabel");
		OnStatsChanged();
	}

	public void SetCharStats(CharacterStats stats)
	{
		_charStats = stats;
		
		if (!_charStats.IsConnected(CharacterStats.SignalName.StatsChanged, Callable.From(OnStatsChanged)))
		{
			_charStats.StatsChanged += OnStatsChanged;
		}

		OnStatsChanged();
	}

	private void OnStatsChanged()
	{
		if (charStats == null) return;
		if (manaLabel == null) return;

		if (!IsInstanceValid(manaLabel)) return;

		manaLabel.Text = $"{charStats.mana}/{charStats.maxMana}";
	}

}
