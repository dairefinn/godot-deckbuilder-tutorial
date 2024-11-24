namespace DeckBuilder;

using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SaveGame : Resource
{

    public static readonly string SAVE_PATH = "user://savegame.tres";

    [Export] public ulong rngSeed;
    [Export] public ulong rngState;
    [Export] public RunStats runStats;
    [Export] public CharacterStats charStats;
    [Export] public CardPile currentDeck;
    [Export] public int currentHealth;
    [Export] public Array<Relic> relics;
    [Export] public Array<Array<Room>> mapData;
    [Export] public Room lastRoom;
    [Export] public int floorsClimbed;
    [Export] public bool wasOnMap;

    public void SaveData()
    {
        Error err = ResourceSaver.Save(this, SAVE_PATH);
        if (err != Error.Ok)
        {
            GD.PrintErr("Couldn't save the game!");
            return;
        }
    }

    public static SaveGame LoadData()
    {
        if (FileAccess.FileExists(SAVE_PATH))
        {
            return ResourceLoader.Load<SaveGame>(SAVE_PATH);
        }

        return null;
    }

    public static void DeleteData()
    {
        if (FileAccess.FileExists(SAVE_PATH))
        {
            DirAccess.RemoveAbsolute(SAVE_PATH);
        }
    }

}