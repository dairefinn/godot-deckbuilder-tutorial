namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class MapRoom : Area2D
{

	private partial class RoomIconData : Resource
	{
		public Texture2D icon;
		public Vector2 iconScale;

		public RoomIconData(string iconPath, Vector2 iconScale)
		{
			if (iconPath != null)
			{
				icon = GD.Load<Texture2D>(iconPath);
			}
			this.iconScale = iconScale;
		}
	}

	private static readonly Dictionary<Room.Type, RoomIconData> ICONS = new Dictionary<Room.Type, RoomIconData>()
	{
		{ Room.Type.NOT_ASSIGNED, new RoomIconData(null, Vector2.One) },
		{ Room.Type.MONSTER, new RoomIconData("res://art/tile_0103.png", Vector2.One) },
		{ Room.Type.TREASURE, new RoomIconData("res://art/tile_0089.png", Vector2.One) },
		{ Room.Type.CAMPFIRE, new RoomIconData("res://art/player_heart.png", new Vector2(0.6f, 0.6f)) },
		{ Room.Type.SHOP, new RoomIconData("res://art/gold.png", new Vector2(0.6f, 0.6f)) },
		{ Room.Type.BOSS, new RoomIconData("res://art/tile_0105.png", new Vector2(1.25f, 1.25f)) }
	};

	[Signal] public delegate void ClickedEventHandler(Room room);
	[Signal] public delegate void SelectedEventHandler(Room room);

	public Sprite2D sprite2D;
	public Line2D line2D;
	public AnimationPlayer animationPlayer;

	public bool available {
		get => _available;
		set => SetAvailable(value);
	}
	private bool _available = true;

	public Room room {
		get => _room;
		set => SetRoom(value);
	}
	private Room _room;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Visuals/Sprite2D");
		line2D = GetNode<Line2D>("Visuals/Line2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		InputEvent += OnInputEvent;
	}

	public void SetAvailable(bool value)
	{
		_available = value;

		if (value == true)
		{
			animationPlayer.Play("highlight");
		}
		else if (!room.selected)
		{
			animationPlayer.Play("RESET");
		}
	}

	public void SetRoom(Room value)
	{
		_room = value;
		Position = value.position;
		line2D.RotationDegrees = GD.RandRange(0, 360);
		sprite2D.Texture = ICONS[value.type].icon;
		sprite2D.Scale = ICONS[value.type].iconScale;
	}

	public void ShowSelected()
	{
		line2D.Modulate = Colors.White;
	}

	public void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (!available || !@event.IsActionPressed("left_mouse"))
		{
			return;
		}

		room.selected = true;
		EmitSignal(SignalName.Clicked, room);
		animationPlayer.Play("select");
	}

	// Called by the AnimationPlayer when the "select" animation finishes
	public void OnMapRoomSelected()
	{
		EmitSignal(MapRoom.SignalName.Selected, room);
	}

}
