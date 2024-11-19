namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class CrabAttackAction : EnemyAction
{

    [Export] int damage = 7;

    public override void PerformAction()
    {
        base.IsPerformable();

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
        tween.TweenCallback(Callable.From(() => damageEffect.Execute(targetArray)));
        tween.TweenInterval(0.25f);
        tween.TweenProperty(enemy, "global_position", start, 0.4f);

        tween.Finished += () => {
            Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
        };
    }

    public override bool IsPerformable()
    {
        if (enemy == null) return false;
        if (target == null) return false;

        return true;
    }

    public override void UpdateIntentText()
    {
		if (target is not Player player) return;

        int modifiedDamage = player.modifierHandler.GetModifiedValue(damage, Modifier.Type.DMG_TAKEN);
        intent.currentText = string.Format(intent.baseText, modifiedDamage);
    }

}
