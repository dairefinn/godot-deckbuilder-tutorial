using Godot;

public partial class Hand : HBoxContainer
{
	
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child is CardUI cardUI)
			{
				cardUI.parent = this;
				cardUI.ReparentRequested += OnReparentRequested;
			}
		}
	}

	private void OnReparentRequested(CardUI card)
	{
		card.Reparent(this);
	}

}
