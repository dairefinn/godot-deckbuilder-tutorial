namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;

public partial class StatusHandler : GridContainer
{

	public static readonly PackedScene STATUS_UI = GD.Load<PackedScene>("res://scenes/status_handler/status_ui.tscn");
	public static readonly float STATUS_APPLY_INTERVAL = 0.25f;

	[Signal] public delegate void StatusesAppliedEventHandler(Status.Type type);

	[Export] public Node2D statusOwner;


	public void ApplyStatusesByType(Status.Type type)
	{
		if (!IsInstanceValid(this)) return;
		if (type == Status.Type.EVENT_BASED) return;

		Array<Status> statusQueue = new(GetAllStatuses().Where(status => status.type == type));
		
		if (statusQueue.Count == 0)
		{
			EmitSignal(SignalName.StatusesApplied, (int)type);
			return;
		}

		Tween tween = CreateTween();
		foreach (Status status in statusQueue)
		{
			tween.TweenCallback(Callable.From(() => status.ApplyStatus(statusOwner)));
			tween.TweenInterval(STATUS_APPLY_INTERVAL);
		}

		tween.Finished += () => EmitSignal(SignalName.StatusesApplied, (int)type);
	}
	
	public void AddStatus(Status status)
	{
		// Add it if it's new
		if (!HasStatus(status.id))
		{
			StatusUI newStatusUI = STATUS_UI.Instantiate<StatusUI>();
			AddChild(newStatusUI);
			newStatusUI.status = status;
			newStatusUI.status.StatusApplied += OnStatusApplied;
			newStatusUI.status.InitializeStatus(statusOwner);
			return;
		}

		bool stackable = status.stackType != Status.StackType.NONE;

		// If it's unique and we already have it, we can return
		if (!status.canExpire && !stackable) return;

		// If it's duration-stackable, expand it
		if (status.canExpire && status.stackType == Status.StackType.DURATION)
		{
			GetStatus(status.id).duration += status.duration;
			return;
		}

		// If it's stackable, stack it
		if (status.stackType == Status.StackType.INTENSITY)
		{
			GetStatus(status.id).stacks += status.stacks;
		}
	}

	public bool HasStatus(string id)
	{
		foreach (StatusUI statusUI in GetChildren())
		{
			if (statusUI.status.id == id)
			{
				return true;
			}
		}

		return false;
	}

	public Status GetStatus(string id)
	{
		foreach (StatusUI statusUI in GetChildren())
		{
			if (statusUI.status.id == id)
			{
				return statusUI.status;
			}
		}

		return null;
	}

	public Array<Status> GetAllStatuses()
	{
		Array<Status> statuses = new();

		foreach (StatusUI statusUI in GetChildren())
		{
			statuses.Add(statusUI.status);
		}

		return statuses;
	}

	public void OnStatusApplied(Status status)
	{
		if (status.canExpire)
		{
			status.duration -= 1;
		}
	}

}

