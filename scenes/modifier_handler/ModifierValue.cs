namespace DeckBuilder;

using Godot;

public partial class ModifierValue : Node
{

	public enum Type
	{
		PERCENT_BASED,
		FLAT
	}

	[Export] public Type type;
	[Export] public float percentValue;
	[Export] public int flatValue;
	[Export] public string source;

	public static ModifierValue CreateNewModifier(string modifierSource, Type whatType)
	{
        ModifierValue newModifier = new()
        {
            source = modifierSource,
            type = whatType
        };

		return newModifier;
    }

}

