namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class ToxicGhostAttackAction : EnemyAction
{

	public static readonly Card TOXIN = GD.Load<Card>("res://common_cards/toxic.tres");

	[Export] public int damage = 8;

	public override void PerformAction()
	{
		if (enemy == null) return;
		if (target == null) return;

        if (target is not Player player) return;

        Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quint);
		Vector2 start = enemy.GlobalPosition;
		Vector2 end = target.GlobalPosition + Vector2.Right * 32;

		DamageEffect damageEffect = new();
		Array<Node> targetArray = new() { target };
		int modifiedDamage = enemy.modifierHandler.GetModifiedValue(damage, Modifier.Type.DMG_DEALT);

		damageEffect.amount = modifiedDamage;
		damageEffect.sound = sound;
		
		tween.TweenProperty(enemy, "global_position", end, 0.4f);
		tween.TweenCallback(Callable.From(() => {
			damageEffect.Execute(targetArray);
		}));
		tween.TweenCallback(Callable.From(() => {
			player.stats.drawPile.AddCard(TOXIN.Duplicate() as Card);
		}));
		tween.TweenInterval(0.25f);
		tween.TweenProperty(enemy, "global_position", start, 0.4f);

		tween.Finished += () => {
			Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
		};
	}

	public override void UpdateIntentText()
	{
		if (target is not Player player) return;

		int modifiedDamage = player.modifierHandler.GetModifiedValue(damage, Modifier.Type.DMG_TAKEN);
		int finalDamage = enemy.modifierHandler.GetModifiedValue(modifiedDamage, Modifier.Type.DMG_DEALT);
		intent.currentText = string.Format(intent.baseText, finalDamage);
	}

}

