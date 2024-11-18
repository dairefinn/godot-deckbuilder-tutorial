namespace DeckBuilder;

using Godot;

public partial class ModifierHandler : Node
{

	public bool HasModifier(Modifier.Type type)
	{
		foreach (Node child in GetChildren())
		{
			if (child is not Modifier modifier) continue;
			if (modifier.type == type)
			{
				return true;
			}
		}

		return false;
	}

	public Modifier GetModifier(Modifier.Type type)
	{
		foreach (Node child in GetChildren())
		{
			if (child is not Modifier modifier) continue;
			if (modifier.type == type)
			{
				return modifier;
			}
		}

		return null;
	}

	public int GetModifiedValue(int baseValue, Modifier.Type type)
	{
		Modifier modifier = GetModifier(type);

		if (modifier == null) return baseValue;

		return modifier.GetModifiedValue(baseValue);
	}

}
