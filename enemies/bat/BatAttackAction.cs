namespace DeckBuilder;

using System.Collections.Generic;
using Godot;

public partial class BatAttackAction : EnemyAction
{
	
    [Export] int damage = 4;

	public override void PerformAction()
	{
		if (enemy == null) return;
		if (target == null) return;

		Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quint);
		Vector2 start = enemy.GlobalPosition;
		Vector2 end = target.GlobalPosition + Vector2.Right * 32;
		DamageEffect damageEffect = new()
		{
			amount = damage,
			sound = sound
		};
		List<Node> targetArray = new() { target };

		tween.TweenProperty(enemy, "global_position", end, 0.4f);
		tween.TweenCallback(Callable.From(() => {
			damageEffect.Execute(targetArray);
			SoundPlayer.TryPlayOnInstance("SFXPlayer", sound);
		}));
		tween.TweenInterval(0.35f);
		tween.TweenCallback(Callable.From(() => {
			damageEffect.Execute(targetArray);
			SoundPlayer.TryPlayOnInstance("SFXPlayer", sound);
		}));
		tween.TweenInterval(0.25f);
		tween.TweenProperty(enemy, "global_position", start, 0.4f);
		tween.Finished += () => {
			Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
		};
	}

}

