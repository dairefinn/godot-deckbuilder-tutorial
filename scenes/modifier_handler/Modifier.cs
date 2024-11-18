namespace DeckBuilder;

using Godot;

public partial class Modifier : Node
{
	
	public enum Type
	{
		DMG_DEALT,
		DMG_TAKEN,
		CARD_COST,
		SHOP_COST,
		NO_MODIFIER
	}

	[Export] public Type type;

	public ModifierValue GetValue(string source)
	{
		foreach (Node child in GetChildren())
		{
			if (child is not ModifierValue modifierValue) continue;
			if (modifierValue.source == source)
			{
				return modifierValue;
			}
		}

		return null;
	}

	public void AddNewValue(ModifierValue value)
	{
		ModifierValue existingValue = GetValue(value.source);

		if (existingValue == null)
		{
			AddChild(value);
		}
		else
		{
			existingValue.flatValue = value.flatValue;
			existingValue.percentValue = value.percentValue;
		}
	}

	public void RemoveValue(string source)
	{
		ModifierValue modifierValue = GetValue(source);

		if (modifierValue != null)
		{
			modifierValue.QueueFree();
		}
	}

	public void ClearValues()
	{
		foreach (Node child in GetChildren())
		{
			if (child is ModifierValue modifierValue)
			{
				modifierValue.QueueFree();
			}
		}
	}

	public int GetModifiedValue(int baseValue)
	{
		int flatResult = baseValue;
		float percentResult = 1.0f;

		// Apply flat modifiers first
		foreach (Node child in GetChildren())
		{
			if (child is not ModifierValue modifierValue) continue;
			if (modifierValue.type == ModifierValue.Type.FLAT)
			{
				flatResult += modifierValue.flatValue;
			}
		}

		// Apply % modifiers next
		foreach (Node child in GetChildren())
		{
			if (child is not ModifierValue modifierValue) continue;
			if (modifierValue.type == ModifierValue.Type.PERCENT_BASED)
			{
				percentResult += modifierValue.percentValue;
			}
		}

		return Mathf.FloorToInt(flatResult * percentResult);
	}

}
