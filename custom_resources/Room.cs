namespace DeckBuilder;

using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Room : Resource
{

    public enum Type {
        NOT_ASSIGNED,
        MONSTER,
        TREASURE,
        CAMPFIRE,
        SHOP,
        BOSS
    }

    [Export] public Type type = Type.NOT_ASSIGNED;
    [Export] public int row;
    [Export] public int column;
    [Export] public Vector2 position;
    [Export] public Array<Room> nextRooms = new();
    [Export] public bool selected = false;

    public override string ToString()
    {
        return string.Format("{0} ({1})", column, type.ToString()[0]);
    }

}
