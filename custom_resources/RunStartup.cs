namespace DeckBuilder;

using Godot;

[GlobalClass]
public partial class RunStartup : Resource
{

    public enum Type
    {
        NEW_RUN,
        CONTINUED_RUN
    }

    [Export] public Type type;
    [Export] public CharacterStats pickedCharacter;

}
