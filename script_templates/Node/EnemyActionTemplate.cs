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

    // If the enemy has dynamic intent text, you can override the base behavior here
    // eg. for attack actions, the Player's DMG TAKEN modifier modifies the resulting damage number.
    public override void UpdateIntentText()
    {
        if (target is not Player player) return;

        int modifiedDamage = player.modifierHandler.GetModifiedValue(6, Modifier.Type.DMG_TAKEN);
        intent.currentText = string.Format(intent.baseText, modifiedDamage);
    }

}
