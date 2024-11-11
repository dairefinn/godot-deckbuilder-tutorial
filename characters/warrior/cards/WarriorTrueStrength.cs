namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class WarriorTrueStrength : Card
{

	public override void ApplyEffects(Array<Node> targets)
	{
		GD.Print("WarriorTrueStrength card has been played");
	}

}

