// meta-name: EnemyAction
// meta-description: An action which can be performed by an enemy during it's turn

namespace DeckBuilder;

using Godot;

public partial class EnemyActionTemplate : EnemyAction
{

    public override void PerformAction()
    {
        if (enemy == null) return;
        if (target == null) return;

        Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quint);
        Vector2 start = enemy.GlobalPosition;
        Vector2 end = target.GlobalPosition + Vector2.Right * 32;

        SoundPlayer.TryPlayOnInstance("SFXPlayer", sound);

        Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, enemy);
    }
}
