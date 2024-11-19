namespace DeckBuilder;

using Godot;
using Godot.Collections;


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
		Array<Node> targetArray = new() { target };

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
		tween.TweenInterval(0.35f);
		tween.TweenProperty(enemy, "global_position", start, 0.4f);
		tween.Finished += () => {
			Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
		};
	}

    public override void UpdateIntentText()
    {
		if (target is not Player player) return;

        int modifiedDamage = player.modifierHandler.GetModifiedValue(damage, Modifier.Type.DMG_TAKEN);
        intent.currentText = string.Format(intent.baseText, modifiedDamage);
    }

}

