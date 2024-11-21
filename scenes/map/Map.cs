namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class Map : Node2D
{

    private static readonly int SCROLL_SPEED = 15;
    private static readonly PackedScene MAP_ROOM = GD.Load<PackedScene>("res://scenes/map/map_room.tscn");
    private static readonly PackedScene MAP_LINE = GD.Load<PackedScene>("res://scenes/map/map_line.tscn");

    public MapGenerator mapGenerator;
    public Node2D lines;
    public Node2D rooms;
    public Node2D visuals;
    public Camera2D camera2d;

    public Array<Array<Room>> mapData;
    public int floorsClimbed;
    public Room lastRoom;
    public float cameraEdgeY;

    public override void _Ready()
    {
        mapGenerator = GetNode<MapGenerator>("MapGenerator");
        lines = GetNode<Node2D>("%Lines");
        rooms = GetNode<Node2D>("%Rooms");
        visuals = GetNode<Node2D>("Visuals");
        camera2d = GetNode<Camera2D>("Camera2D");

        cameraEdgeY = MapGenerator.Y_DIST * (MapGenerator.FLOORS - 1);
    }

    public override void _Input(InputEvent @event)
    {
        if (!Visible) return;

        Vector2 newPosition = camera2d.Position;

        if (@event.IsActionPressed("scroll_up"))
        {
            newPosition.Y -= SCROLL_SPEED;
        }
        else if (@event.IsActionPressed("scroll_down"))
        {
            newPosition.Y += SCROLL_SPEED;
        }

        newPosition.Y = Mathf.Clamp(newPosition.Y, -cameraEdgeY, 0);

        camera2d.Position = newPosition;
    }

    public void GenerateNewMap()
    {
        floorsClimbed = 0;
        mapData = mapGenerator.GenerateMap();
        CreateMap();
    }

    public void CreateMap()
    {
        foreach (Array<Room> floor in mapData)
        {
            foreach (Room room in floor)
            {
                if (room.nextRooms.Count > 0)
                {
                    SpawnRoom(room);
                }
            }
        }

        // Boss room has no next room so we spawn it separately
        var middle = Mathf.FloorToInt(MapGenerator.MAP_WIDTH * 0.5f);
        SpawnRoom(mapData[MapGenerator.FLOORS - 1][middle]);

        int mapWidthPixels = MapGenerator.X_DIST * (MapGenerator.MAP_WIDTH - 1);
        visuals.Position = new Vector2((GetViewportRect().Size.X - mapWidthPixels) / 2, GetViewportRect().Size.Y / 2);
    }

    public void UnlockFloor(int floor)
    {
        foreach (Node mapRoomNode in rooms.GetChildren())
        {
            if (mapRoomNode is not MapRoom mapRoom) continue;
            if (mapRoom.room.row == floor)
            {
                mapRoom.available = true;
            }
        }
    }

    public void UnlockNextRooms()
    {
        foreach (Node mapRoomNode in rooms.GetChildren())
        {
            if (mapRoomNode is not MapRoom mapRoom) continue;
            if (lastRoom.nextRooms.Contains(mapRoom.room))
            {
                mapRoom.available = true;
            }
        }
    }

    public void ShowMap()
    {
        Show();
        camera2d.Enabled = true;
    }

    public void HideMap()
    {
        Hide();
        camera2d.Enabled = false;
    }

    public void SpawnRoom(Room room)
    {
        MapRoom newMapRoom = MAP_ROOM.Instantiate<MapRoom>();
        rooms.AddChild(newMapRoom);
        newMapRoom.room = room;
        newMapRoom.Selected += OnMapRoomSelected;
        ConnectLines(room);

        if (room.selected && room.row < floorsClimbed)
        {
            newMapRoom.ShowSelected();
        }
    }

    public void ConnectLines(Room room)
    {
        if (room.nextRooms.Count == 0) return;

        foreach (Room nextRoom in room.nextRooms)
        {
            Line2D newMapLine = MAP_LINE.Instantiate<Line2D>();
            newMapLine.AddPoint(room.position);
            newMapLine.AddPoint(nextRoom.position);
            lines.AddChild(newMapLine);
        }
    }

    public void OnMapRoomSelected(Room room)
    {
        foreach (Node mapRoomNode in rooms.GetChildren())
        {
            if (mapRoomNode is not MapRoom mapRoom) continue;
            if (mapRoom.room.row == room.row)
            {
                mapRoom.available = false;
            }
        }

        lastRoom = room;
        floorsClimbed += 1;
        Events.Instance.EmitSignal(Events.SignalName.MapExited, room);
    }

}
